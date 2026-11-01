using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.UseCases.GetProductLookup;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Mawthooq.Infrastructure.AdoNet.Shared;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProductLookup
{
    internal sealed class GetProductLookupQueryRepository : IGetProductLookupQueryRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public GetProductLookupQueryRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }
        public async Task<IReadOnlyList<GetProductLookupItemDto>> GetProductLookupAsync(GetProductLookupQuery query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            var whereSql = string.IsNullOrWhiteSpace(query.SearchText)
                ? ""
                : "WHERE p.TradeName LIKE @SearchText ESCAPE '\'";

            string sql = $"""
                    SELECT TOP (@MaxResults)
                        p.ProductId, p.TradeName, p.Sku, ib.Value AS InternalBarcode

                    FROM
                        dbo.Products p

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

                    {whereSql}

                    ORDER BY
                        p.TradeName, p.ProductId;
                    """;

            await using SqlCommand command = new(sql, connection);
            command.Parameters.Add("@MaxResults", SqlDbType.Int).Value = query.MaxResults;

            if (!string.IsNullOrWhiteSpace(query.SearchText))
                command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value = $"{SqlLike.Escape(query.SearchText)}%";

            List<GetProductLookupItemDto> items = [];

            await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                items.Add(new GetProductLookupItemDto(
                    reader.GetGuid(reader.GetOrdinal("ProductId")),
                    reader.GetString(reader.GetOrdinal("TradeName")),
                    reader.GetString(reader.GetOrdinal("Sku")),
                    reader.GetString(reader.GetOrdinal("InternalBarcode"))));
            }

            return items;
        }
    }
}
