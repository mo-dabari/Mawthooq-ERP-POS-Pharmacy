using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.ActivateProduct;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.ActivateProduct
{
    public class ActivateProductUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ActivateProductUseCase _sut;

        public ActivateProductUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _sut = new ActivateProductUseCase(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_ActivateProduct_And_ReturnUpdatedResponse()
        {
            var productValid = ProductTestFactory.CreateValidProduct();
            productValid.Deactivate();

            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productValid.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((productValid, null));

            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
            .Setup(pr => pr.UpdateAsync(productValid, requestRowVersion, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newRowVersion);

            var request = new ActivateProductRequest(productValid.ProductId.ToDto(), requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.NewStatus.Should().Be(Status.Active);
            response.RowVersion.Should().BeEquivalentTo(newRowVersion);

            _productRepositoryMock.Verify(pr => pr.UpdateAsync(productValid, requestRowVersion, It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_ProductNotFound()
        {
            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = new ActivateProductRequest(ProductId.New().ToDto(), [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);
            await act.Should().ThrowAsync<InvalidOperationException>();

            _productRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Product>(), It.IsAny<Byte[]>(), It.IsAny<CancellationToken>())
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
