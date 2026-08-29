using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.UpdateProductInformation;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.UpdateProductInformation
{
    public class UpdateProductInformationUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly UpdateProductInformationUseCase _sut;

        public UpdateProductInformationUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _sut = new UpdateProductInformationUseCase(
                _productRepositoryMock.Object,
                _categoryRepositoryMock.Object);
        }


        [Fact]
        public async Task Should_UpdateProductInformation_And_ReturnUpdatedResponse()
        {
            var product = ProductTestFactory.CreateValidProduct();
            var newCategory = Category.Create(CategoryId.New(), "Cosmetics", "New category");
            var newImage = new ImageDto("products/1/new-image.png", "new-image.png", "image/png");

            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(newCategory.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((newCategory, null));

            _productRepositoryMock
                .Setup(r => r.UpdateAsync(product, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new UpdateProductInformationRequest(
                product.ProductId.ToDto(),
                "Panadol Ultra",
                newCategory.CategoryId.ToDto(),
                newImage,
                CanBeReturnedIfOpened: false,
                NearExpiryThresholdInDays: 45,
                requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.Name.Should().Be("Panadol Ultra");
            response.CategoryId.Value.Should().Be(newCategory.CategoryId.Value);
            response.Image.StorageKey.Should().Be(newImage.StorageKey);
            response.CanBeReturnedIfOpened.Should().BeFalse();
            response.NearExpiryThresholdInDays.Should().Be(45);
            response.RowVersion.Should().BeEquivalentTo(newRowVersion);

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

            var request = CreateValidRequest(ProductId.New(), CategoryId.New());

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<InvalidOperationException>();

            _categoryRepositoryMock.Verify(
                r => r.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_CategoryNotFound()
        {
            var product = ProductTestFactory.CreateValidProduct();
            var newCategoryId = CategoryId.New();

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(newCategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = CreateValidRequest(product.ProductId, newCategoryId);

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

        private static UpdateProductInformationRequest CreateValidRequest(ProductId productId, CategoryId categoryId)
        {
            return new UpdateProductInformationRequest(
                productId.ToDto(),
                "Panadol Ultra",
                categoryId.ToDto(),
                new ImageDto("products/1/new-image.png", "new-image.png", "image/png"),
                CanBeReturnedIfOpened: false,
                NearExpiryThresholdInDays: 45,
                RowVersion: [1, 1, 1]);
        }
    }
}
