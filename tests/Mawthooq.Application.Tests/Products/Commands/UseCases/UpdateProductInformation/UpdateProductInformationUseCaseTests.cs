using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Categories.Exceptions;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.UpdateProductInformation;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Events;
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
                NearExpiryThresholdInDays: Product.MinDays,
                requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.TradeName.Should().Be("Panadol Ultra");
            response.CategoryId.Value.Should().Be(newCategory.CategoryId.Value);
            response.Image.StorageKey.Should().Be(newImage.StorageKey);
            response.CanBeReturnedIfOpened.Should().BeFalse();
            response.NearExpiryThresholdInDays.Should().Be(Product.MinDays);
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

            var request = CreateValidRequest(ProductId.New(), CategoryId.New());

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ProductNotFoundException>();

            _categoryRepositoryMock.Verify(
                r => r.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_ThrowCategoryNotFoundException_When_CategoryNotFound()
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

            await act.Should().ThrowAsync<CategoryNotFoundException>();

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }


        [Theory]
        [InlineData(Product.MinDays - 1)]
        [InlineData(Product.MaxDays + 1)]
        public async Task Should_ThrowValidationException_And_LeaveProductUntouched_When_ThresholdIsOutOfRange(int threshold)
        {
            var product = ProductTestFactory.CreateValidProduct();
            var category = Category.Create(CategoryId.New(), "Cosmetics", "New category");
            var originalName = product.TradeName;
            var originalThreshold = product.NearExpiryThresholdInDays;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(category.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((category, null));

            var request = new UpdateProductInformationRequest(
                product.ProductId.ToDto(), "Panadol Ultra", category.CategoryId.ToDto(),
                new ImageDto("products/1/new-image.png", "new-image.png", "image/png"),
                CanBeReturnedIfOpened: false, NearExpiryThresholdInDays: threshold, RowVersion: [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ValidationException>();

            product.TradeName.Should().Be(originalName);
            product.NearExpiryThresholdInDays.Should().Be(originalThreshold);
            product.DomainEvents.Should().BeEmpty();

            _productRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(500)]
        public async Task Should_KeepThresholdZero_And_IgnoreRequestValue_When_ProductDoesNotTrackExpiry(int requestedThreshold)
        {
            var product = ProductTestFactory.CreateProductWithoutExpiryTracking();
            var category = Category.Create(CategoryId.New(), "Devices", "Devices category");
            byte[] rowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _productRepositoryMock.Setup(r => r.GetByIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((product, null));

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(category.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((category, null));

            _productRepositoryMock.Setup(r => r.UpdateAsync(product, rowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new UpdateProductInformationRequest(
                product.ProductId.ToDto(), "Digital Thermometer Pro", category.CategoryId.ToDto(),
                new ImageDto("products/1/new-image.png", "new-image.png", "image/png"),
                CanBeReturnedIfOpened: false, NearExpiryThresholdInDays: requestedThreshold, RowVersion: rowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.TradeName.Should().Be("Digital Thermometer Pro");
            response.NearExpiryThresholdInDays.Should().Be(0);
            product.DomainEvents.OfType<ProductNearExpiryThresholdChangedEvent>().Should().BeEmpty();
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
                NearExpiryThresholdInDays: 100,
                RowVersion: [1, 1, 1]);
        }
    }
}
