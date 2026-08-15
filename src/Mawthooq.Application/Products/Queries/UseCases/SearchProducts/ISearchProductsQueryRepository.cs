using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.SearchProducts
{
    public interface ISearchProductsQueryRepository
    {
        Task<IReadOnlyList<ProductSearchResultDto>> SearchProductsAsync(
            SearchProductsQuery query,
            CancellationToken cancellationToken = default);
    }
}
