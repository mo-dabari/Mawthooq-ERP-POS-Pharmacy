using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.AddingNewBarcodeToUnit;
using Mawthooq.Application.Products.Commands.UseCases.AddPackagingVersion;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.Services;
using Mawthooq.Domain.Products.ValueObjects;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.AddPackagingVersion
{
    public class AddPackagingVersionUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IInternalBarcodeSequenceGenerator> _barcodeGeneratorMock;
        private readonly ProductFactory _productFactory;
        private readonly AddPackagingVersionUseCase _sut;

        public AddPackagingVersionUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _barcodeGeneratorMock = new Mock<IInternalBarcodeSequenceGenerator>();
            _productFactory = new ProductFactory(_barcodeGeneratorMock.Object);
            _sut = new AddPackagingVersionUseCase(_productRepositoryMock.Object, _productFactory);
        }

        [Fact]
        public async Task Should_AddPackagingVersion_And_ReturnLatestVersion()
        {
            var product = ProductTestFactory.CreateValidProduct();
            _barcodeGeneratorMock
                .Setup(g => g.GetNextBarcodeSequenceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(9000L);

            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _productRepositoryMock
                .Setup(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new AddPackagingVersionRequest(
                product.ProductId.ToDto(),
                PackagingTemplate.SingleUnit,
                new PackagingInformation(null, null, null, null, null, null, UnitOfMeasure.Capsule),
                requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.PackagingVersion.Version.Should().Be(2);
            response.RowVersion.Should().BeEquivalentTo(newRowVersion);

            product.PackagingVersions.Should().HaveCount(2);
            product.PackagingVersions
                .Single(pv => pv.Version == 2)
                .Units.Should().ContainSingle(u => u.Unit == UnitOfMeasure.Capsule);

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_ProductNotFound()
        {
            var product = ProductTestFactory.CreateValidProduct();

            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = new AddPackagingVersionRequest(
                ProductId.New().ToDto(),
                PackagingTemplate.SingleUnit,
                new PackagingInformation(null, null, null, null, null, null, UnitOfMeasure.Capsule),
                [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<InvalidOperationException>();

            _productRepositoryMock
            .Verify(pr => pr.UpdateAsync(
                It.IsAny<Product>(),
                It.IsAny<Byte[]>(),
                It.IsAny<CancellationToken>())
                , Times.Never);
        }

        [Fact]
        public async Task Should_ThrowArgumentNullException_When_RequestIsNull()
        {
            Func<Task> act = () => _sut.ExecuteAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
