using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.UseCases.ScanProductBarcode;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.ScanProductBarcode
{
    public sealed class ScanProductBarcodeQueryRepository : IScanProductBarcodeQueryRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public ScanProductBarcodeQueryRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }

        public async Task<ScanProductBarcodeDto?> ScanProductBarcodeAsync(
            ScanProductBarcodeQuery query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            const string sql = """
            SELECT
                p.ProductId, p.TradeName, p.Sku, pv.PackagingVersionId,
                pu.Unit, pu.ConversionRate, b.Value AS MatchedBarcode, p.Status

            FROM
                dbo.Barcodes b

            INNER JOIN
                dbo.ProductUnits pu

            ON pu.ProductUnitId = b.ProductUnitId

            INNER JOIN
                dbo.PackagingVersions pv

            ON pv.PackagingVersionId = pu.PackagingVersionId

            INNER JOIN
                dbo.Products p

            ON p.ProductId = pv.ProductId

            WHERE b.Value = @Barcode;
            """;

            await using SqlCommand command = new(sql, connection);
            command.Parameters.Add("@Barcode", SqlDbType.NVarChar, 50).Value = query.Barcode;

            await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
                return null;

            return new ScanProductBarcodeDto(
                reader.GetGuid(reader.GetOrdinal("ProductId")),
                reader.GetString(reader.GetOrdinal("TradeName")),
                reader.GetString(reader.GetOrdinal("Sku")),
                reader.GetGuid(reader.GetOrdinal("PackagingVersionId")),
                ((UnitOfMeasure)reader.GetByte(reader.GetOrdinal("Unit"))).ToString(),
                reader.GetInt32(reader.GetOrdinal("ConversionRate")),
                reader.GetString(reader.GetOrdinal("MatchedBarcode")),
                (Status)reader.GetByte(reader.GetOrdinal("Status")));
        }
    }
}
