using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Pagination;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProducts
{
    public interface IGetProductsQueryRepository
    {
        Task<PagedResult<GetProductsItemDto>> GetProductsAsync(
            GetProductsQuery query,
            CancellationToken cancellationToken = default);
    }
}
