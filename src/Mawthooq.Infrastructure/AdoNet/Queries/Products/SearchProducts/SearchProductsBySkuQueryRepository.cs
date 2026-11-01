using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.SearchProducts
{
    internal sealed class SearchProductsBySkuQueryRepository : ProductSearchQueryRepositoryBase
    {
        private const string BySkuSql = SelectWithPrimaryBarcode + """

        WHERE p.Sku LIKE @SearchText ESCAPE '\'
        ORDER BY p.Sku, p.ProductId;
        """;

        public SearchProductsBySkuQueryRepository(IDbConnectionProvider connectionProvider) : base(connectionProvider) { }
        public override ProductSearchBy SearchBy => ProductSearchBy.Sku;
        protected override string Sql => BySkuSql;
    }
}
