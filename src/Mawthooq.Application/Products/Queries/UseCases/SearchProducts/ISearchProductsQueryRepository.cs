using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Enums;

namespace Mawthooq.Application.Products.Queries.UseCases.SearchProducts
{
    public interface ISearchProductsQueryRepository
    {
        ProductSearchBy SearchBy { get; }
        Task<IReadOnlyList<ProductSearchResultDto>> SearchProductsAsync(
            SearchProductsQuery query,
            CancellationToken cancellationToken = default);
    }
}
