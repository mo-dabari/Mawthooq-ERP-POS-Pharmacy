using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.SearchProducts
{
    public class SearchProductsHandler
    {
        private readonly ISearchProductsQueryRepository _searchProductsQueryRepository;

        public SearchProductsHandler(ISearchProductsQueryRepository searchProductsQueryRepository)
        {
            ArgumentNullException.ThrowIfNull(searchProductsQueryRepository, nameof(searchProductsQueryRepository));

            _searchProductsQueryRepository = searchProductsQueryRepository;
        }

        public Task<IReadOnlyList<ProductSearchResultDto>> HandleAsync(
            SearchProductsQuery query,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            if (string.IsNullOrWhiteSpace(query.SearchText))
                throw new ArgumentException("Search text must not be empty.", nameof(query));

            return _searchProductsQueryRepository.SearchProductsAsync(query, cancellationToken);
        }
    }
}
