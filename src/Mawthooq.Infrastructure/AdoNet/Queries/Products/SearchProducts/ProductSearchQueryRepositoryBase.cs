using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Enums;
using Mawthooq.Application.Products.Queries.UseCases.SearchProducts;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Mawthooq.Infrastructure.AdoNet.Shared;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.SearchProducts
{
    internal abstract class ProductSearchQueryRepositoryBase : ISearchProductsQueryRepository
    {
        protected const string SelectWithPrimaryBarcode = """

            SELECT TOP (@MaxResults)
                p.ProductId, p.TradeName, p.Sku, p.Status,
                c.Name AS CategoryName,
                ib.Value AS MatchedBarcode

            FROM
                dbo.Products p

            INNER JOIN
                dbo.Categories c
            ON
                c.CategoryId = p.CategoryId

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
                AND
                    pu.Rank = 1

                INNER JOIN
                    dbo.Barcodes b
                ON
                    b.ProductUnitId = pu.ProductUnitId
                AND
                    b.Type = 6

                WHERE
                    pv.ProductId = p.ProductId
                ORDER BY
                    CASE WHEN
                        pv.Status = 1
                        THEN 0
                        ELSE 1
                    END,
                    pv.Version DESC
            ) ib
        """;

        private readonly IDbConnectionProvider _connectionProvider;

        protected ProductSearchQueryRepositoryBase(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }

        public abstract ProductSearchBy SearchBy { get; }
        protected abstract string Sql { get; }

        public async Task<IReadOnlyList<ProductSearchResultDto>> SearchProductsAsync(SearchProductsQuery query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(Sql, connection);
            command.Parameters.Add("@MaxResults", SqlDbType.Int).Value = query.MaxResults;
            command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 256).Value = $"{SqlLike.Escape(query.SearchText)}%";

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
