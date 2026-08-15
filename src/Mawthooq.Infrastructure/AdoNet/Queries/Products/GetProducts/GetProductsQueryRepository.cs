using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Pagination;
using Mawthooq.Application.Products.Queries.UseCases.GetProducs;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProducts
{
    public class GetProductsQueryRepository : IGetProductsQueryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetProductsQueryRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public Task<PagedResult<GetProductsItemDto>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
