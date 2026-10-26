using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.AddingNewBarcodeToUnit;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.AddingNewBarcodeToUnit
{
    public class AddingNewBarcodeToUnitUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly AddingNewBarcodeToUnitUseCase _sut;

        public AddingNewBarcodeToUnitUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();

            _sut = new AddingNewBarcodeToUnitUseCase(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_AddBarcode_And_ReturnUpdatedResponse()
        {
            var product = ProductTestFactory.CreateValidProduct();
            var versionId = product.PackagingVersions[0].PackagingVersionId;
            var barcode = BarcodeEAN13Linear.New("1234567891234");

            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(
                    product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _productRepositoryMock
                .Setup(r => r.ExistsByBarcodeAsync(barcode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _productRepositoryMock
                .Setup(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new AddingNewBarcodeToUnitRequest(
                product.ProductId.ToDto(),
                versionId.ToDto(),
                UnitOfMeasure.Bottle,
                barcode.ToDto(),
                requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.Barcodes.Should().Contain(b => b.Value == barcode.Value);
            response.RowVersion.Should().BeEquivalentTo(newRowVersion);

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()),
                Times.Once);
        }


        [Fact]
        public async Task Should_ThrowValidationException_When_BarcodeIsNull()
        {
            var request = CreateRequestWithBarcodeIsNull();

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ValidationException>();

            _productRepositoryMock
            .Verify(pr => pr.UpdateAsync(
                It.IsAny<Product>(),
                It.IsAny<Byte[]>(),
                It.IsAny<CancellationToken>())
                , Times.Never);
        }


        [Fact]
        public async Task Should_ThrowProductNotFoundException_When_ProductNotFound()
        {
            var product = ProductTestFactory.CreateValidProduct();

            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = CreateValidRequest(product);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ProductNotFoundException>();

            _productRepositoryMock
            .Verify(pr => pr.UpdateAsync(
                It.IsAny<Product>(),
                It.IsAny<Byte[]>(),
                It.IsAny<CancellationToken>())
                , Times.Never);
        }

        [Fact]
        public async Task Should_ThrowDuplicateBarcodeException_When_BarcodeAlreadyExists()
        {
            var product = ProductTestFactory.CreateValidProduct();

            var request = CreateValidRequest(product);

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _productRepositoryMock
                .Setup(r => r.ExistsByBarcodeAsync(It.IsAny<Barcode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<DuplicateBarcodeException>();

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_ThrowArgumentNullException_When_RequestIsNull()
        {
            Func<Task> act = () => _sut.ExecuteAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        private static AddingNewBarcodeToUnitRequest CreateValidRequest(Product product)
        {
            return new AddingNewBarcodeToUnitRequest(
                product.ProductId.ToDto(),
                product.PackagingVersions[0].PackagingVersionId.ToDto(),
                UnitOfMeasure.Bottle,
                BarcodeEAN13Linear.New("1234567891234").ToDto(),
                [1, 1, 1]);
        }

        private static AddingNewBarcodeToUnitRequest CreateRequestWithBarcodeIsNull()
        {
            var product = ProductTestFactory.CreateValidProduct();

            return new AddingNewBarcodeToUnitRequest(
                product.ProductId.ToDto(),
                product.PackagingVersions[0].PackagingVersionId.ToDto(),
                UnitOfMeasure.Bottle,
                null!,
                [1, 1, 1]);
        }
    }
}
