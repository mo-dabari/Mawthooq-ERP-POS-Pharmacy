using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Queries.Products.GetProducts
{
    internal sealed record DetailsRow(
        Guid ProductId, string TradeName, string Sku, string CategoryName, string ManufacturerName, string LocalAgentName,
        bool CanBeReturnedIfOpened, byte ProductStatus, string ImageStorageKey,
        Guid PackagingVersionId, int Version, byte PackagingVersionStatus,
        byte Unit, int ConversionRate, byte Rank, string BarcodeValue, byte BarcodeType)
    {
        public static DetailsRow FromReader(SqlDataReader reader) => new(
            reader.GetGuid(reader.GetOrdinal("ProductId")),
            reader.GetString(reader.GetOrdinal("TradeName")),
            reader.GetString(reader.GetOrdinal("Sku")),
            reader.GetString(reader.GetOrdinal("CategoryName")),
            reader.GetString(reader.GetOrdinal("ManufacturerName")),
            reader.GetString(reader.GetOrdinal("LocalAgentName")),
            reader.GetBoolean(reader.GetOrdinal("CanBeReturnedIfOpened")),
            reader.GetByte(reader.GetOrdinal("ProductStatus")),
            reader.GetString(reader.GetOrdinal("ImageStorageKey")),
            reader.GetGuid(reader.GetOrdinal("PackagingVersionId")),
            reader.GetInt32(reader.GetOrdinal("Version")),
            reader.GetByte(reader.GetOrdinal("PackagingVersionStatus")),
            reader.GetByte(reader.GetOrdinal("Unit")),
            reader.GetInt32(reader.GetOrdinal("ConversionRate")),
            reader.GetByte(reader.GetOrdinal("Rank")),
            reader.GetString(reader.GetOrdinal("BarcodeValue")),
            reader.GetByte(reader.GetOrdinal("BarcodeType")));
    }
}
