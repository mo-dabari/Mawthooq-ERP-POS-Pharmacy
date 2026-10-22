using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Categories.Commands.UseCases.UpdateCategory;
using Mawthooq.Application.Categories.Exceptions;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Categories.ValueObjects;
using Moq;

namespace Mawthooq.Application.Tests.Categories.Commands.UseCases.UpdateCategory
{
    public class UpdateCategoryUseCaseTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly UpdateCategoryUseCase _sut;

        public UpdateCategoryUseCaseTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _sut = new UpdateCategoryUseCase(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_UpdateCategory_And_ReturnUpdatedResponse()
        {
            var category = Category.Create(CategoryId.New(), "Pharmaceuticals", "General category");
            byte[] requestRowVersion = [1, 1, 1];
            byte[] newRowVersion = [2, 2, 2];

            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(category.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((category, null));

            _categoryRepositoryMock
                .Setup(r => r.UpdateAsync(category, requestRowVersion, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRowVersion);

            var request = new UpdateCategoryRequest(
                category.CategoryId.ToDto(),
                "Cosmetics",
                "Updated description",
                requestRowVersion);

            var response = await _sut.ExecuteAsync(request);

            response.Name.Should().Be("Cosmetics");
            response.Description.Should().Be("Updated description");
            response.NewRowVersion.Should().BeEquivalentTo(newRowVersion);

            _categoryRepositoryMock.Verify(
                r => r.UpdateAsync(category, requestRowVersion, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_ThrowCategoryNotFoundException_When_CategoryNotFound()
        {
            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = new UpdateCategoryRequest(
                CategoryId.New().ToDto(),
                "Cosmetics",
                "Updated description",
                [1, 1, 1]);

            Func<Task> act = () => _sut.ExecuteAsync(request);

            await act.Should().ThrowAsync<CategoryNotFoundException>();

            _categoryRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Category>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
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
