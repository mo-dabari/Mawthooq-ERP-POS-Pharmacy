using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.UseCases.SearchProducts;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.SearchProducts
{
    public sealed class SearchProductsQueryRepository : ISearchProductsQueryRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public SearchProductsQueryRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }

        public async Task<IReadOnlyList<ProductSearchResultDto>> SearchProductsAsync(
            SearchProductsQuery query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            const string sql = """
            SELECT
                TOP (@MaxResults)
                p.ProductId, p.TradeName, p.Sku, c.Name AS CategoryName, p.Status, ib.Value AS MatchedBarcode

            FROM
                dbo.Products p

            INNER JOIN
                dbo.Categories c

            ON c.CategoryId = p.CategoryId

            CROSS APPLY
            (
                SELECT
                    TOP (1) b.Value

                FROM
                    dbo.PackagingVersions pv

                INNER JOIN
                    dbo.ProductUnits pu

                ON
                    pu.PackagingVersionId = pv.PackagingVersionId

                INNER JOIN
                    dbo.Barcodes b

                ON
                    b.ProductUnitId = pu.ProductUnitId

                WHERE
                    pv.ProductId = p.ProductId
                AND
                    pv.Status = 1
                AND
                    pu.Rank = 1
                AND
                    b.Type = 6
            ) ib

            WHERE
                p.TradeName LIKE @SearchText OR p.Sku LIKE @SearchText

            ORDER BY
                p.TradeName;
            """;

            await using SqlCommand command = new(sql, connection);
            command.Parameters.Add("@MaxResults", SqlDbType.Int).Value = query.MaxResults;
            command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value = $"{query.SearchText}%";

            List<ProductSearchResultDto> results = [];

            await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                results.Add(new ProductSearchResultDto(
                    reader.GetGuid(reader.GetOrdinal("ProductId")),
                    reader.GetString(reader.GetOrdinal("TradeName")),
                    reader.GetString(reader.GetOrdinal("Sku")),
                    reader.GetString(reader.GetOrdinal("MatchedBarcode")),
                    reader.GetString(reader.GetOrdinal("CategoryName")),
                    (Status)reader.GetByte(reader.GetOrdinal("Status"))));
            }
            return results;
        }
    }
}
