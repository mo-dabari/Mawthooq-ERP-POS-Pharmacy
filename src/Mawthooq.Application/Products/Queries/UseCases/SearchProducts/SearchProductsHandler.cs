using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Enums;

namespace Mawthooq.Application.Products.Queries.UseCases.SearchProducts
{
    public class SearchProductsHandler
    {
        private readonly IReadOnlyDictionary<ProductSearchBy, ISearchProductsQueryRepository> _repositories;
        public SearchProductsHandler(IEnumerable<ISearchProductsQueryRepository> repositories)
        {
            ArgumentNullException.ThrowIfNull(repositories, nameof(repositories));

            _repositories = repositories.ToDictionary(repository => repository.SearchBy);
        }

        public Task<IReadOnlyList<ProductSearchResultDto>> HandleAsync(
            SearchProductsQuery query,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            SearchProductsQueryValidator.Validate(query);

            return _repositories.TryGetValue(query.SearchBy, out ISearchProductsQueryRepository? repository)
            ? repository.SearchProductsAsync(query, cancellationToken)
            : throw new NotSupportedException($"No search implementation is registered for '{query.SearchBy}'.");
        }
    }
}
