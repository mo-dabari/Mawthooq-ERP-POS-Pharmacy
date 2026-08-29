using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Products.Commands.Mappers;
using Mawthooq.Application.Products.Commands.UseCases.ChangeProductCategory;
using Mawthooq.Application.Tests.Products.TestHelpersProduct;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
using Moq;

namespace Mawthooq.Application.Tests.Products.Commands.UseCases.ChangeProductCategory
{
    public class ChangeProductCategoryUseCaseTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly ChangeProductCategoryUseCase _sut;

        public ChangeProductCategoryUseCaseTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _sut = new ChangeProductCategoryUseCase(_productRepositoryMock.Object, _categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_ChangeCategory_When_NewCategoryExistsAndActive()
        {
            var productValid = ProductTestFactory.CreateValidProduct();

            var newCategoryId = CategoryId.New();
            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            var request = new ChangeProductCategoryRequest(
                productValid.ProductId.ToDto(),
                newCategoryId.ToDto(),
                requestRowVersion);

            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productValid.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((productValid, null));

            _categoryRepositoryMock
                .Setup(cr => cr.ExistsAndActiveAsync(newCategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productRepositoryMock
                .Setup(pr => pr.UpdateAsync(productValid, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var response = await _sut.ExecuteAsync(request);

            productValid.CategoryId.Should().Be(newCategoryId);
            response.NewCategoryId.Should().Be(newCategoryId.ToDto());
            response.NewRowVersion.Should().BeEquivalentTo(newRowVersion);

            _productRepositoryMock
                .Verify(pr => pr.UpdateAsync(
                    productValid,
                    requestRowVersion,
                    It.IsAny<CancellationToken>())
                    , Times.Once);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_NewCategoryNotFoundOrInactive()
        {
            var productValid = ProductTestFactory.CreateValidProduct();

            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productValid.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((productValid, null));

            _categoryRepositoryMock
                .Setup(cr => cr.ExistsAndActiveAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var request = CreateValidRequest();

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
        public async Task Should_ThrowInvalidOperationException_When_ProductNotFound()
        {
            _productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<ProductId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = CreateValidRequest();

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

        private static ChangeProductCategoryRequest CreateValidRequest()
        {
            return new ChangeProductCategoryRequest(
                ProductId.New().ToDto(),
                CategoryId.New().ToDto(),
                [1, 1, 1]);
        }
    }
}
