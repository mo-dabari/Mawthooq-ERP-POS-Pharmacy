using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.DisallowReturnIfOpened;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.DisallowReturnIfOpened
{
    public class DisallowReturnIfOpenedUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly DisallowReturnIfOpenedUseCase _sut;

        public DisallowReturnIfOpenedUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _sut = new DisallowReturnIfOpenedUseCase(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_DisallowReturn_And_ReturnUpdatedResponse()
        {
            var product = ProductTestFactory.CreateValidProduct();
            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, (byte[]?)null));

            _productRepositoryMock
                .Setup(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new DisallowReturnIfOpenedRequest(product.ProductId.ToDto(), requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.CanBeReturnedIfOpened.Should().BeFalse();
            response.RowVersion.Should().BeEquivalentTo(newRowVersion);

            product.CanBeReturnedIfOpened.Should().BeFalse();

            _productRepositoryMock
            .Verify(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>())
                , Times.Once);
        }

        [Fact]
        public async Task Should_ThrowProductNotFoundException_When_ProductNotFound()
        {
            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(((Product?)null, (byte[]?)null));

            var request = new DisallowReturnIfOpenedRequest(ProductId.New().ToDto(), [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ProductNotFoundException>();

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>())
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
