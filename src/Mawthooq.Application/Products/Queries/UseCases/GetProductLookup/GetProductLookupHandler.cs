using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductLookup
{
    public class GetProductLookupHandler
    {
        private readonly IGetProductLookupQueryRepository _getProductLookupQueryRepository;

        public GetProductLookupHandler(IGetProductLookupQueryRepository getProductLookupQueryRepository)
        {
            ArgumentNullException.ThrowIfNull(getProductLookupQueryRepository, nameof(getProductLookupQueryRepository));

            _getProductLookupQueryRepository = getProductLookupQueryRepository;
        }
        public Task<IReadOnlyList<GetProductLookupItemDto>> HandleAsync(
            GetProductLookupQuery query,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            return _getProductLookupQueryRepository.GetProductLookupAsync(query, cancellationToken);
        }
    }
}
