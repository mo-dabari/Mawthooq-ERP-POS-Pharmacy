using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Pagination;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProducts
{
    public class GetProductListHandler
    {
        private readonly IGetProductsQueryRepository _getProductsQueryRepository;

        public GetProductListHandler(IGetProductsQueryRepository getProductsQueryRepository)
        {
            ArgumentNullException.ThrowIfNull(getProductsQueryRepository, nameof(getProductsQueryRepository));

            _getProductsQueryRepository = getProductsQueryRepository;
        }

        public Task<PagedResult<GetProductsItemDto>> HandleAsync(
            GetProductsQuery query,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            GetProductsQueryValidator.Validate(query);

            return _getProductsQueryRepository.GetProductsAsync(query, cancellationToken);
        }
    }
}
