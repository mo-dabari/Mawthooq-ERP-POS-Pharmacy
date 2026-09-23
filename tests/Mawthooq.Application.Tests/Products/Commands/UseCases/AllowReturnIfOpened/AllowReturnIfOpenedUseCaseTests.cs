using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.AllowReturnIfOpened;
using Mawthooq.Application.Products.Commands.UseCases.DiscontinueProduct;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.AllowReturnIfOpened
{
    public class AllowReturnIfOpenedUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly AllowReturnIfOpenedUseCase _sut;

        public AllowReturnIfOpenedUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _sut = new AllowReturnIfOpenedUseCase(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_AllowReturn_And_ReturnUpdatedResponse()
        {
            var product = ProductTestFactory.CreateValidProduct();

            product.DisallowReturnIfOpened();

            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _productRepositoryMock
                .Setup(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new AllowReturnIfOpenedRequest(
                product.ProductId.ToDto(),
                requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.CanBeReturnedIfOpened.Should().BeTrue();
            response.TradeName.Should().Be(product.TradeName);
            response.RowVersion.Should().BeEquivalentTo(newRowVersion);

            product.CanBeReturnedIfOpened.Should().BeTrue();

            _productRepositoryMock
            .Verify(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>())
                , Times.Once);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_ProductNotFound()
        {
            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(((Product?)null, (byte[]?)null));

            var request = new AllowReturnIfOpenedRequest(ProductId.New().ToDto(), [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ProductNotFoundException>();

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
