using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductDetails
{
    public class GetProductDetailsHandler
    {
        private readonly IGetProductDetailsQueryRepository
        _getProductDetailsQueryRepository;

        public GetProductDetailsHandler(IGetProductDetailsQueryRepository getProductDetailsQueryRepository)
        {
            ArgumentNullException.ThrowIfNull(getProductDetailsQueryRepository, nameof(getProductDetailsQueryRepository));

            _getProductDetailsQueryRepository = getProductDetailsQueryRepository;
        }

        public Task<GetProductDetailsDto?> HandleAsync(GetProductDetailsQuery query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            return _getProductDetailsQueryRepository.GetProductDetailsAsync(query, cancellationToken);
        }
    }
}
