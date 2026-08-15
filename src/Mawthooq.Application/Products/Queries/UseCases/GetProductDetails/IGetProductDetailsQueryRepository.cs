using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.UseCases.GetProductById;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductDetails
{
    public interface IGetProductDetailsQueryRepository
    {
        Task<GetProductDetailsDto?> GetProductDetailsAsync(
            GetProductDetailsQuery query,
            CancellationToken cancellationToken = default);
    }
}
