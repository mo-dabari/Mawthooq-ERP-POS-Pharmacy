using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.RemoveCompanyBarcodeToUnit;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.RemoveCompanyBarcodeToUnit
{
    public class RemoveCompanyBarcodeToUnitUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly RemoveCompanyBarcodeToUnitUseCase _sut;

        public RemoveCompanyBarcodeToUnitUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _sut = new RemoveCompanyBarcodeToUnitUseCase(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_RemoveBarcode_And_ReturnUpdatedResponse()
        {
            var product = ProductTestFactory.CreateValidProduct();
            var versionId = product.PackagingVersions[0].PackagingVersionId;
            var barcode = BarcodeEAN13Linear.New("1234567891234");

            product.AddCompanyBarcodeToVersion(versionId, UnitOfMeasure.Bottle, barcode);

            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _productRepositoryMock
                .Setup(r => r.ExistsByBarcodeAsync(It.IsAny<Barcode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productRepositoryMock
                .Setup(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new RemoveCompanyBarcodeToUnitRequest(
                product.ProductId.ToDto(),
                versionId.ToDto(),
                UnitOfMeasure.Bottle,
                barcode.ToDto(),
                requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.Barcodes.Should().NotContain(b => b.Value == barcode.Value);
            response.NewRowVersion.Should().BeEquivalentTo(newRowVersion);

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_ThrowBarcodeNotFoundException_When_BarcodeNotFound()
        {
            var product = ProductTestFactory.CreateValidProduct();
            var versionId = product.PackagingVersions[0].PackagingVersionId;
            var barcode = BarcodeEAN13Linear.New("9999999999999");

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, (byte[]?)null));

            _productRepositoryMock
                .Setup(r => r.ExistsByBarcodeAsync(It.IsAny<Barcode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var request = new RemoveCompanyBarcodeToUnitRequest(
                product.ProductId.ToDto(),
                versionId.ToDto(),
                UnitOfMeasure.Bottle,
                barcode.ToDto(),
                [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<BarcodeNotFoundException>();

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_ThrowProductNotFoundException_When_ProductNotFound()
        {
            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(((Product?)null, (byte[]?)null));

            var request = new RemoveCompanyBarcodeToUnitRequest(
                ProductId.New().ToDto(),
                PackagingVersionId.New().ToDto(),
                UnitOfMeasure.Bottle,
                BarcodeEAN13Linear.New("1234567891234").ToDto(),
                [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Should_ThrowArgumentNullException_When_RequestIsNull()
        {
            Func<Task> act = () => _sut.ExecuteAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
