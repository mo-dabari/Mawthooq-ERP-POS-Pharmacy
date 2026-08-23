using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.Common.Enums;
using Mawthooq.Application.Products.Queries.Common.Pagination;
using Mawthooq.Application.Products.Queries.UseCases.GetProducs;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts;
using Mawthooq.Application.Products.Queries.UseCases.GetProducts.Enums;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProducts
{
    public class GetProductsQueryRepository : IGetProductsQueryRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public GetProductsQueryRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }
        public async Task<PagedResult<GetProductsItemDto>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            var whereClauses = new List<string>();

            var command = new SqlCommand { Connection = connection };

            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                whereClauses.Add("p.TradeName LIKE @SearchText");
                command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value = $"%{query.SearchText}%";
            }

            if (query.FilterByCategoryId is Guid categoryId)
            {
                whereClauses.Add("c.CategoryName = @CategoryName");
                command.Parameters.Add("@CategoryId", SqlDbType.UniqueIdentifier).Value = categoryId;
            }

            if (query.FilterByManufacturerId is Guid manufacturerId)
            {
                whereClauses.Add("m.ManufacturerName = @ManufacturerName");
                command.Parameters.Add("@ManufacturerId", SqlDbType.UniqueIdentifier).Value = manufacturerId;
            }

            if (query.FilterByStatus == ProductStatusFilter.ActiveOnly)
                whereClauses.Add("p.Status = 1");

            else if (query.FilterByStatus == ProductStatusFilter.InactiveOnly)
                whereClauses.Add("p.Status = 2");

            string whereSql = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";
            string orderColumn = MapSortColumn(query.SortBy);
            string orderDirection = MapSortDirection(query.SortDirection);
            int offset = (query.PageNumber - 1) * query.PageSize;

            command.CommandText = $"""
                            SELECT
                                p.ProductId, p.Sku, p.TradeName, p.Status,
                                c.Name AS CategoryName,
                                m.Name AS ManufacturerName,
                                la.Name AS LocalAgentName,
                                ib.Value AS InternalBarcode ,
                                COUNT(*) OVER() AS TotalCount

                            FROM
                                dbo.Products p

                            INNER JOIN
                                dbo.Categories c ON c.CategoryId = p.CategoryId

                            INNER JOIN
                                dbo.Manufacturers m ON m.ManufacturerId = p.ManufacturerId

                            INNER JOIN
                                dbo.LocalAgents la ON la.LocalAgentId = p.LocalAgentId

                            CROSS APPLY
                            (
                                SELECT TOP (1) b.Value

                                FROM
                                    dbo.PackagingVersions pv

                                INNER JOIN
                                    dbo.ProductUnits pu ON pu.PackagingVersionId = pv.PackagingVersionId

                                INNER JOIN
                                    dbo.Barcodes b ON b.ProductUnitId = pu.ProductUnitId

                                WHERE
                                    pv.ProductId = p.ProductId AND pv.Status = 1 AND pu.Rank = 1 AND b.Type = 6
                            ) ib

                            {whereSql}

                            ORDER BY
                                {orderColumn} {orderDirection}

                            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                            """;


            command.Parameters.Add("@Offset", SqlDbType.Int).Value = offset;
            command.Parameters.Add("@PageSize", SqlDbType.Int).Value = query.PageSize;

            await using (command)
            {
                await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                var items = new List<GetProductsItemDto>();
                int totalCount = 0;

                while (await reader.ReadAsync(cancellationToken))
                {
                    items.Add(new GetProductsItemDto(
                        reader.GetGuid(reader.GetOrdinal("ProductId")),
                        reader.GetString(reader.GetOrdinal("InternalBarcode")),
                        reader.GetString(reader.GetOrdinal("Sku")),
                        reader.GetString(reader.GetOrdinal("TradeName")),
                        reader.GetString(reader.GetOrdinal("CategoryName")),
                        reader.GetString(reader.GetOrdinal("ManufacturerName")),
                        reader.GetString(reader.GetOrdinal("LocalAgentName")),
                        (Status)reader.GetByte(reader.GetOrdinal("Status"))));

                    totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
                }

                return new PagedResult<GetProductsItemDto>(items, totalCount, query.PageNumber, query.PageSize);
            }
        }
        private static string MapSortColumn(ProductSortBy sortBy) => sortBy switch
        {
            ProductSortBy.TradeName => "p.TradeName",
            ProductSortBy.Sku => "p.Sku",
            ProductSortBy.Manufacturer => "m.Name",
            ProductSortBy.Category => "c.Name",
            ProductSortBy.Status => "p.Status",
            ProductSortBy.Barcode => "ib.Value",
            _ => "p.TradeName"
        };

        private static string MapSortDirection(SortDirection direction) =>
            direction == SortDirection.Desc ? "DESC" : "ASC";
    }
}
