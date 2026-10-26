using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Categories.Exceptions;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.LocalAgents.DTOs;
using Mawthooq.Application.Manufacturers.DTOs;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Application.Products.Exceptions;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.Services;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.CreateProduct
{
    public class CreateProductUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IInternalBarcodeSequenceGenerator> _barcodeGeneratorMock;
        private readonly ProductFactory _productFactory;
        private readonly CreateProductUseCase _sut;

        public CreateProductUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _barcodeGeneratorMock = new Mock<IInternalBarcodeSequenceGenerator>();
            _productFactory = new ProductFactory(_barcodeGeneratorMock.Object);

            _sut = new CreateProductUseCase(
                _productRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _productFactory);
        }

        [Fact]
        public async Task Should_CreateProduct_RequestIsValid()
        {
            var category = Category.Create(CategoryId.New(), "Pharmaceuticals", "General category");

            _categoryRepositoryMock
                .Setup(cr => cr.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((category, null));

            _productRepositoryMock
                .Setup(pr => pr.ExistsBySkuAsync(It.IsAny<SKU>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _barcodeGeneratorMock
                .Setup(g => g.GetNextBarcodeSequenceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1000);

            Product? capturedProduct = null;
            byte[] expectedRowVersion = [1, 2, 3];

            _productRepositoryMock
                .Setup(pr => pr.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>((p, _) => capturedProduct = p)
                .ReturnsAsync((Product? p, CancellationToken _) => (p, expectedRowVersion));

            var request = CreateValidRequest(category.CategoryId);

            var response = await _sut.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.TradeName.Should().Be("Panadol Extra");
            response.RowVersion.Should().BeEquivalentTo(expectedRowVersion);

            capturedProduct.Should().NotBeNull();
            capturedProduct?.CategoryId.Should().Be(category.CategoryId);

            _productRepositoryMock
                .Verify(pr => pr.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_AddCompanyBarcodes_When_UserBarcodesProvided()
        {
            var category = Category.Create(CategoryId.New(), "Pharmaceuticals", "General category");

            _categoryRepositoryMock
                .Setup(cr => cr.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((category, null));

            _productRepositoryMock
                .Setup(pr => pr.ExistsBySkuAsync(It.IsAny<SKU>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _barcodeGeneratorMock
                .Setup(g => g.GetNextBarcodeSequenceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1000);

            byte[] expectedRowVersion = [1, 2, 3];

            _productRepositoryMock
                .Setup(pr => pr.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product? p, CancellationToken _) => (p, expectedRowVersion));

            var request = CreateValidRequestWithBarcods(category.CategoryId);

            var response = await _sut.ExecuteAsync(request);

            var barcodes = response.PackagingVersions.First()
                .Units.Single(u => u.Unit == UnitOfMeasure.Bottle)
                .Barcodes;

            var expectedUserBarcodeValue = request.UserBarcodes?
                .Single(b => b.Barcode.Type == BarcodeType.EAN13)
                .Barcode.Value;

            barcodes.Should().HaveCount(2);
            barcodes.Should().Contain(b => b.Type == BarcodeType.Internal);
            barcodes.Should().Contain(b => b.Type == BarcodeType.EAN13 &&
                b.Value == expectedUserBarcodeValue);

            _productRepositoryMock
            .Verify(pr => pr.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Theory]
        [InlineData(Product.MinDays - 1)]
        [InlineData(Product.MaxDays + 1)]
        public async Task Should_ThrowValidationException_When_ChangeNearExpiryThresholdInvalid(int threshold)
        {
            var request = CreateRequestWithNearExpiryThresholdInDaysInvalid(CategoryId.New(), threshold);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<ValidationException>();

            _productRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_ThrowCategoryNotFoundException_When_CategoryNotFound()
        {
            _categoryRepositoryMock
                .Setup(cr => cr.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = CreateValidRequest(CategoryId.New());

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<CategoryNotFoundException>();

            _productRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_ThrowDuplicateSkuException_When_SkuAlreadyExists()
        {
            var category = Category.Create(CategoryId.New(), "Pharmaceuticals", "General category");

            _categoryRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((category, null));

            _productRepositoryMock.Setup(pr => pr.ExistsBySkuAsync(It.IsAny<SKU>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

            var request = CreateValidRequest(category.CategoryId);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<DuplicateSkuException>();

            _barcodeGeneratorMock.Verify(br => br.GetNextBarcodeSequenceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _productRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_ThrowDuplicateBarcodeException_When_BarcodeAlreadyExists()
        {
            var category = Category.Create(CategoryId.New(), "Pharmaceuticals", "General category");

            _categoryRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((category, null));

            _productRepositoryMock.Setup(pr => pr.ExistsBySkuAsync(It.IsAny<SKU>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

            _productRepositoryMock.Setup(pr => pr.ExistsByBarcodeAsync(It.IsAny<Barcode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

            var request = CreateValidRequestWithBarcods(category.CategoryId);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<DuplicateBarcodeException>();

            _barcodeGeneratorMock.Verify(br => br.GetNextBarcodeSequenceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _productRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_NotSavedProduct()
        {
            var category = Category.Create(CategoryId.New(), "Pharmaceuticals", "General category");

            _categoryRepositoryMock
                .Setup(cr => cr.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((category, null));

            _productRepositoryMock
                .Setup(pr => pr.ExistsBySkuAsync(It.IsAny<SKU>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _barcodeGeneratorMock
                .Setup(g => g.GetNextBarcodeSequenceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1000);

            _productRepositoryMock
                .Setup(pr => pr.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = CreateValidRequest(category.CategoryId);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Should_ThrowArgumentNullException_When_RequestIsNull()
        {
            Func<Task> act = () => _sut.ExecuteAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        private static CreateProductRequest CreateValidRequest(CategoryId categoryId)
        {
            var basicInfo = new BasicInformation(
                "Panadol Extra",
                SKU.Create("PANADOL-0001").ToDto(),
                categoryId.ToDto(),
                new ManufacturerIdDto(Guid.NewGuid()),
                new LocalAgentIdDto(Guid.NewGuid()),
                PackagingTemplate.SingleUnit,
                new ImageDto("products/1/default.png", "default.png", "image/png"),
                true,
                true,
                Product.MinDays);

            var packagingInfo = new PackagingInformation(
                null, null, null, null, null, null, UnitOfMeasure.Bottle);

            return new CreateProductRequest(basicInfo, packagingInfo, null);
        }

        private static CreateProductRequest CreateRequestWithNearExpiryThresholdInDaysInvalid(CategoryId categoryId, int threshold)
        {
            var basicInfo = new BasicInformation(
                "Panadol Extra",
                SKU.Create("PANADOL-0001").ToDto(),
                categoryId.ToDto(),
                new ManufacturerIdDto(Guid.NewGuid()),
                new LocalAgentIdDto(Guid.NewGuid()),
                PackagingTemplate.SingleUnit,
                new ImageDto("products/1/default.png", "default.png", "image/png"),
                true,
                true,
                threshold);

            var packagingInfo = new PackagingInformation(
                null, null, null, null, null, null, UnitOfMeasure.Bottle);

            return new CreateProductRequest(basicInfo, packagingInfo, null);
        }

        private static CreateProductRequest CreateValidRequestWithBarcods(CategoryId categoryId)
        {
            List<UserUnitBarcodeDto> userUnitBarcodes = new()
            {
                new(UnitOfMeasure.Bottle, BarcodeEAN13Linear.New("1234567891234").ToDto())
            };

            var request = CreateValidRequest(categoryId);

            return new CreateProductRequest(request.ProductInformation, request.Packaging, userUnitBarcodes);
        }
    }
}
