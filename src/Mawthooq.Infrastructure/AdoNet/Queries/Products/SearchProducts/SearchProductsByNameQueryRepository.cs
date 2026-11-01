using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.SearchProducts
{
    internal sealed class SearchProductsByNameQueryRepository : ProductSearchQueryRepositoryBase
    {
        private const string ByNameSql = SelectWithPrimaryBarcode + """

        WHERE p.TradeName LIKE @SearchText ESCAPE '\'
        ORDER BY p.TradeName, p.ProductId;
        """;

        public SearchProductsByNameQueryRepository(IDbConnectionProvider connectionProvider) : base(connectionProvider) { }
        public override ProductSearchBy SearchBy => ProductSearchBy.Name;
        protected override string Sql => ByNameSql;
    }
}
