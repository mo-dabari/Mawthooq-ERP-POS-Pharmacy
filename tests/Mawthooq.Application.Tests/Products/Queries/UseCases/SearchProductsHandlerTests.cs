using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Products.Queries.Common.Enums;
using Mawthooq.Application.Products.Queries.Common.Pagination;
using Mawthooq.Application.Products.Queries.UseCases.SearchProducts;
using Moq;
using Xunit;

namespace Mawthooq.Application.Tests.Products.Queries.UseCases
{
    public class SearchProductsHandlerTests
    {
        private static Mock<ISearchProductsQueryRepository> CreateRepository(ProductSearchBy searchBy)
        {
            var mock = new Mock<ISearchProductsQueryRepository>();
            mock.SetupGet(r => r.SearchBy).Returns(searchBy);
            mock.Setup(r => r.SearchProductsAsync(It.IsAny<SearchProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductSearchResultDto>());
            return mock;
        }

        [Fact]
        public async Task Should_UseOnlyTheRepositoryOfTheRequestedSearchType()
        {
            var byName = CreateRepository(ProductSearchBy.Name);
            var bySku = CreateRepository(ProductSearchBy.Sku);

            var sut = new SearchProductsHandler([byName.Object, bySku.Object]);

            var query = new SearchProductsQuery("pan", ProductSearchBy.Sku);

            await sut.HandleAsync(query);

            bySku.Verify(r => r.SearchProductsAsync(query, It.IsAny<CancellationToken>()), Times.Once);
            byName.Verify(r => r.SearchProductsAsync(It.IsAny<SearchProductsQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData("", ProductSearchBy.Name, 20)]
        [InlineData("pan", (ProductSearchBy)0, 20)]
        [InlineData("pan", ProductSearchBy.Name, 0)]
        [InlineData("pan", ProductSearchBy.Name, PagingDefaults.MaxResultsLimit + 1)]
        public async Task Should_ThrowValidationException_And_NotCallAnyRepository_When_QueryIsInvalid(
        string text, ProductSearchBy searchBy, int maxResults)
        {
            var byName = CreateRepository(ProductSearchBy.Name);
            var sut = new SearchProductsHandler([byName.Object]);

            Func<Task> act = () => sut.HandleAsync(new SearchProductsQuery(text, searchBy, maxResults));

            await act.Should().ThrowAsync<ValidationException>();
            byName.Verify(r => r.SearchProductsAsync(It.IsAny<SearchProductsQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_ThrowNotSupportedException_When_NoRepositoryIsRegisteredForTheSearchType()
        {
            var sut = new SearchProductsHandler([CreateRepository(ProductSearchBy.Name).Object]);

            Func<Task> act = () => sut.HandleAsync(new SearchProductsQuery("pan", ProductSearchBy.Barcode));

            await act.Should().ThrowAsync<NotSupportedException>();
        }

        [Fact]
        public void Should_Throw_When_TwoRepositoriesShareTheSameSearchType()
        {
            Action act = () => new SearchProductsHandler(
                [CreateRepository(ProductSearchBy.Name).Object, CreateRepository(ProductSearchBy.Name).Object]);

            act.Should().Throw<ArgumentException>();
        }
    }
}
