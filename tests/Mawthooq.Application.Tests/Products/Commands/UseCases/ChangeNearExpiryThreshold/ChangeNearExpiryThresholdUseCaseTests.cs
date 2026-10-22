using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.ChangeNearExpiryThreshold;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.ChangeNearExpiryThreshold
{
    public class ChangeNearExpiryThresholdUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ChangeNearExpiryThresholdUseCase _sut;

        public ChangeNearExpiryThresholdUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _sut = new ChangeNearExpiryThresholdUseCase(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_ChangeThreshold_And_ReturnUpdatedResponse()
        {
            var product = ProductTestFactory.CreateValidProduct();
            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _productRepositoryMock
                .Setup(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new ChangeNearExpiryThresholdRequest(
                product.ProductId.ToDto(), Product.MinDays, requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.NearExpiryThresholdInDays.Should().Be(Product.MinDays);
            response.Name.Should().Be(product.TradeName);
            response.RowVersion.Should().BeEquivalentTo(newRowVersion);

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_ThrowProductNotFoundException_When_ProductNotFound()
        {
            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = new ChangeNearExpiryThresholdRequest(
                ProductId.New().ToDto(), Product.MinDays, [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ProductNotFoundException>();

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData(Product.MinDays - 1)]
        [InlineData(Product.MaxDays + 1)]
        public async Task Should_ThrowValidationException_When_ChangeNearExpiryThresholdInvalid(int threshold)
        {
            var request = new ChangeNearExpiryThresholdRequest(
                ProductId.New().ToDto(), threshold, [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ValidationException>();

            _productRepositoryMock.Verify(
                r => r.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()),
                Times.Never);

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
