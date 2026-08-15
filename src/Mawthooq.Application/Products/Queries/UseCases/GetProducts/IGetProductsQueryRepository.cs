using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Pagination;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProducs
{
    public interface IGetProductsQueryRepository
    {
        Task<PagedResult<GetProductsItemDto>> GetProductsAsync(
            GetProductsQuery query,
            CancellationToken cancellationToken = default);
    }
}
