using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Repositories
{
    internal sealed record ProductRow(
    Guid ProductId, string TradeName, string Sku, Guid CategoryId, Guid ManufacturerId, Guid LocalAgentId,
    bool RequiresExpiryTracking, bool CanBeReturnedIfOpened, int NearExpiryThresholdInDays,
    byte ProductStatus, string ImageStorageKey, string ImageFileName, string ImageMimeType, byte[] RowVersion,
    Guid PackagingVersionId, int Version, byte PackagingVersionStatus,
    int ProductUnitId, byte Unit, int ConversionRate, byte Rank,
    string BarcodeValue, byte BarcodeType)
    {
        public static ProductRow FromReader(SqlDataReader reader) => new(
            reader.GetGuid(reader.GetOrdinal("ProductId")),
            reader.GetString(reader.GetOrdinal("TradeName")),
            reader.GetString(reader.GetOrdinal("Sku")),
            reader.GetGuid(reader.GetOrdinal("CategoryId")),
            reader.GetGuid(reader.GetOrdinal("ManufacturerId")),
            reader.GetGuid(reader.GetOrdinal("LocalAgentId")),
            reader.GetBoolean(reader.GetOrdinal("RequiresExpiryTracking")),
            reader.GetBoolean(reader.GetOrdinal("CanBeReturnedIfOpened")),
            reader.GetInt32(reader.GetOrdinal("NearExpiryThresholdInDays")),
            reader.GetByte(reader.GetOrdinal("ProductStatus")),
            reader.GetString(reader.GetOrdinal("ImageStorageKey")),
            reader.GetString(reader.GetOrdinal("ImageFileName")),
            reader.GetString(reader.GetOrdinal("ImageMimeType")),
            (byte[])reader["RowVersion"],
            reader.GetGuid(reader.GetOrdinal("PackagingVersionId")),
            reader.GetInt32(reader.GetOrdinal("Version")),
            reader.GetByte(reader.GetOrdinal("PackagingVersionStatus")),
            reader.GetInt32(reader.GetOrdinal("ProductUnitId")),
            reader.GetByte(reader.GetOrdinal("Unit")),
            reader.GetInt32(reader.GetOrdinal("ConversionRate")),
            reader.GetByte(reader.GetOrdinal("Rank")),
            reader.GetString(reader.GetOrdinal("BarcodeValue")),
            reader.GetByte(reader.GetOrdinal("BarcodeType")));
    }
}
