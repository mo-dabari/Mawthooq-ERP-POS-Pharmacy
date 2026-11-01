using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.SearchProducts
{
    internal sealed class SearchProductsByBarcodeQueryRepository : ProductSearchQueryRepositoryBase
    {
        private const string ByBarcodeSql = """
        SELECT
            TOP (@MaxResults)
            p.ProductId, p.TradeName, p.Sku, c.Name AS CategoryName, p.Status, MIN(b.Value) AS MatchedBarcode

        FROM
            dbo.Barcodes b

        INNER JOIN
            dbo.ProductUnits pu
        ON
            pu.ProductUnitId = b.ProductUnitId

        INNER JOIN
            dbo.PackagingVersions pv
        ON
            pv.PackagingVersionId = pu.PackagingVersionId

        INNER JOIN
            dbo.Products p
        ON
            p.ProductId = pv.ProductId

        INNER JOIN
            dbo.Categories c
        ON
            c.CategoryId = p.CategoryId

        WHERE
            b.Value LIKE @SearchText ESCAPE '\'

        GROUP BY
            p.ProductId, p.TradeName, p.Sku, c.Name, p.Status

        ORDER BY
            p.TradeName, p.ProductId;
        """;

        public SearchProductsByBarcodeQueryRepository(IDbConnectionProvider connectionProvider) : base(connectionProvider) { }
        public override ProductSearchBy SearchBy => ProductSearchBy.Barcode;
        protected override string Sql => ByBarcodeSql;
    }
}
