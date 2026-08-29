using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Categories.Commands.UseCases.CreateCategory;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.SharedKernel.Enums;
using Moq;

namespace Mawthooq.Application.Tests.Categories.Commands.UseCases.CreateCategory
{
    public class CreateCategoryUseCaseTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly CreateCategoryUseCase _sut;

        public CreateCategoryUseCaseTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _sut = new CreateCategoryUseCase(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_CreateCategory_And_ReturnResponse()
        {
            Category? capturedCategory = null;
            byte[] expectedRowVersion = [1, 2, 3];

            _categoryRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
                .Callback<Category, CancellationToken>((c, _) => capturedCategory = c)
                .ReturnsAsync((Category? c, byte[]? _) => (c, expectedRowVersion));

            var request = new CreateCategoryRequest("Pharmaceuticals", "General category");

            var response = await _sut.ExecuteAsync(request);

            response.Name.Should().Be("Pharmaceuticals");
            response.Description.Should().Be("General category");
            response.Status.Should().Be(Status.Active);
            response.RowVersion.Should().BeEquivalentTo(expectedRowVersion);

            capturedCategory.Should().NotBeNull();

            _categoryRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_ThrowInvalidOperationException_When_AddAsyncReturnsNullCategoryAndNullRowVersion()
        {
            _categoryRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, null));

            var request = new CreateCategoryRequest("Pharmaceuticals", "General category");

            Func<Task> act = () => _sut.ExecuteAsync(request);

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
