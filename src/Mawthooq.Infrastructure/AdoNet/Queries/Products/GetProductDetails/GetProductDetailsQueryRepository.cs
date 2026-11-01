using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.UseCases.GetProductDetails;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProducts;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProductDetails
{
    internal sealed class GetProductDetailsQueryRepository : IGetProductDetailsQueryRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public GetProductDetailsQueryRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }

        public async Task<GetProductDetailsDto?> GetProductDetailsAsync(GetProductDetailsQuery query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            ArgumentNullException.ThrowIfNull(query.ProductId, nameof(query.ProductId));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            const string sql = """
                SELECT
                    P.ProductId, P.TradeName, P.Sku, P.CategoryId, P.ManufacturerId, P.LocalAgentId, P.RequiresExpiryTracking,

                    P.CanBeReturnedIfOpened, P.NearExpiryThresholdInDays, P.Status AS ProductStatus,

                    P.ImageStorageKey, P.ImageFileName, P.ImageMimeType, P.RowVersion,

                    Pv.PackagingVersionId, Pv.Version, Pv.Status AS PackagingVersionStatus,

                    Pu.ProductUnitId, Pu.Unit, Pu.ConversionRate, Pu.Rank,

                    Pb.BarcodeId, Pb.Value AS BarcodeValue, Pb.Type AS BarcodeType,

                    C.Name AS CategoryName,

                    M.Name AS ManufacturerName,

                    La.Name AS LocalAgentName

                FROM
                    dbo.Products AS P

                INNER JOIN
                    dbo.PackagingVersions AS Pv
                ON
                    Pv.ProductId = P.ProductId

                INNER JOIN
                    dbo.ProductUnits AS Pu
                ON
                    Pu.PackagingVersionId = Pv.PackagingVersionId

                INNER JOIN
                    dbo.Barcodes AS Pb
                ON
                    Pb.ProductUnitId = Pu.ProductUnitId

                INNER JOIN
                    dbo.Categories AS C
                ON
                    C.CategoryId = P.CategoryId

                INNER JOIN
                    dbo.Manufacturers AS M
                ON
                    M.ManufacturerId = P.ManufacturerId

                INNER JOIN
                    dbo.LocalAgents AS La
                ON
                    La.LocalAgentId = P.LocalAgentId

                WHERE
                    P.ProductId = @ProductId

                ORDER BY
                    Pv.Version, Pu.Rank, Pb.BarcodeId
            """;

            await using SqlCommand command = new(sql, connection);

            command.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = query.ProductId.Value;

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            List<DetailsRow> rows = [];

            while (await reader.ReadAsync(cancellationToken))
            {
                rows.Add(DetailsRow.FromReader(reader));
            }

            return rows.Count == 0 ? null : MapProductDetailsDto(rows);
        }

        private static GetProductDetailsDto MapProductDetailsDto(IReadOnlyList<DetailsRow> rows)
        {
            ArgumentNullException.ThrowIfNull(rows, nameof(rows));
            if (rows.Count == 0)
                throw new ArgumentException("Rows collection cannot be empty.", nameof(rows));

            DetailsRow first = rows[0];

            var packagingVersions = MapPackagingVersions(rows);

            return new GetProductDetailsDto(
                first.ProductId,
                first.TradeName,
                first.Sku,
                first.CategoryName,
                first.ManufacturerName,
                first.LocalAgentName,
                first.CanBeReturnedIfOpened,
                first.NearExpiryThresholdInDays,
                (Status)first.ProductStatus,
                first.ImageStorageKey,
                packagingVersions,
                 first.RowVersion);
        }

        private static IReadOnlyList<PackagingVersionDto> MapPackagingVersions(IReadOnlyList<DetailsRow> rows)
        {
            ArgumentNullException.ThrowIfNull(rows, nameof(rows));

            if (rows.Count == 0)
                throw new ArgumentException("Rows Packaging Versions collection cannot be empty.", nameof(rows));

            return rows
            .GroupBy(pv => pv.PackagingVersionId)
            .Select(versionGroup =>
            {
                DetailsRow versionRow = versionGroup.First();
                var units = MapUnits(versionGroup);

                return new PackagingVersionDto(
                    versionRow.PackagingVersionId,
                    versionRow.Version,
                    ((Status)versionRow.PackagingVersionStatus).ToString(),
                    units);
            }).ToList();
        }

        private static List<ProductUnitDetailsDto> MapUnits(IEnumerable<DetailsRow> versionRows)
        {
            return versionRows
                .GroupBy(r => r.Unit)
                .Select(unitGroup =>
                {
                    DetailsRow unitRow = unitGroup.First();
                    var barcodes = MapBarcodes(unitGroup);

                    return new ProductUnitDetailsDto(
                        ((UnitOfMeasure)unitRow.Unit).ToString(),
                        unitRow.ConversionRate,
                        ((UnitRank)unitRow.Rank).ToString(),
                        barcodes);
                })
                .ToList();
        }

        private static IReadOnlyList<BarcodeDto> MapBarcodes(IEnumerable<DetailsRow> unitGroup)
        {
            return unitGroup
                .Select(r => new BarcodeDto(r.BarcodeValue, ((BarcodeType)r.BarcodeType).ToString()))
                .ToList();
        }
    }
}
