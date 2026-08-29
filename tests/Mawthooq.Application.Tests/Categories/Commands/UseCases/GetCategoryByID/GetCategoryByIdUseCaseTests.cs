using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Categories.UseCases.GetCategoryByID;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Categories.ValueObjects;
using Moq;

namespace Mawthooq.Application.Tests.Categories.Commands.UseCases.GetCategoryByID
{
    public class GetCategoryByIdUseCaseTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly GetCategoryByIdUseCase _sut;

        public GetCategoryByIdUseCaseTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _sut = new GetCategoryByIdUseCase(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_ReturnCategory_When_Found()
        {
            var category = Category.Create(CategoryId.New(), "Pharmaceuticals", "General category");

            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(category.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((category, (byte[]?)null));

            var response = await _sut.ExecuteAsync(category.CategoryId.ToDto());

            response.CategoryId.Value.Should().Be(category.CategoryId.Value);
            response.Name.Should().Be(category.Name);
            response.Description.Should().Be(category.Description);
            response.Status.Should().Be(category.Status);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_CategoryNotFound()
        {
            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<CategoryId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            Func<Task> act = () => _sut.ExecuteAsync(CategoryId.New().ToDto());

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Should_ThrowArgumentNullException_When_RequestIsNull()
        {
            Func<Task> act = () => _sut.ExecuteAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
