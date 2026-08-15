using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductLookup
{
    public interface IGetProductLookupQueryRepository
    {
        Task<IReadOnlyList<GetProductLookupItemDto>> GetProductLookupAsync(
            GetProductLookupQuery query,
            CancellationToken cancellationToken = default);
    }
}
