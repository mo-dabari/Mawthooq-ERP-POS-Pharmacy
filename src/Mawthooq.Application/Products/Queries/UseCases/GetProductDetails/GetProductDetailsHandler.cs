using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.UseCases.GetProductDetails;
using Microsoft.VisualBasic;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductById
{
    public class GetProductByIdHandler
    {
        private readonly IGetProductDetailsQueryRepository
        _getProductDetailsQueryRepository;

        public GetProductByIdHandler(IGetProductDetailsQueryRepository getProductDetailsQueryRepository)
        {
            ArgumentNullException.ThrowIfNull(getProductDetailsQueryRepository, nameof(getProductDetailsQueryRepository));

            _getProductDetailsQueryRepository = getProductDetailsQueryRepository;
        }

        public Task<GetProductDetailsDto?> HanHandleAsyncdle(GetProductDetailsQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            return _getProductDetailsQueryRepository.GetProductDetailsAsync(
                        query,
                        cancellationToken
                        ) ?? throw new InvalidOperationException($"Not Found Product");
        }
    }
}
