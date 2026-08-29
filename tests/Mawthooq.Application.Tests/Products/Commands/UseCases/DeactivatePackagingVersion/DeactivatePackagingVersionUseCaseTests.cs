using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.DeactivatePackagingVersion;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.DeactivatePackagingVersion
{
    public class DeactivatePackagingVersionUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly DeactivatePackagingVersionUseCase _sut;

        public DeactivatePackagingVersionUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _sut = new DeactivatePackagingVersionUseCase(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_DeactivatePackagingVersion_And_ReturnUpdatedResponse()
        {
            var product = ProductTestFactory.CreateValidProduct();
            var versionId = product.PackagingVersions[0].PackagingVersionId;

            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, (byte[]?)null));

            _productRepositoryMock
                .Setup(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new DeactivatePackagingVersionRequest(
                product.ProductId.ToDto(), versionId.ToDto(), requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.Status.Should().Be(Status.Inactive);
            response.RowVersion.Should().BeEquivalentTo(newRowVersion);

            product.PackagingVersions.Single(pv => pv.PackagingVersionId == versionId)
                .Status.Should().Be(Status.Inactive);

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_ProductNotFound()
        {
            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = new DeactivatePackagingVersionRequest(
                ProductId.New().ToDto(), PackagingVersionId.New().ToDto(), [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<InvalidOperationException>();

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
    }
}
