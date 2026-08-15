using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Mappers
{
    public static class ProductMapper
    {
        // public static Product ToProductDomain(this SqlDataReader sqlData)
        // {
        //     int productId = sqlData.GetInt32(sqlData.GetOrdinal("ProductId"));
        //     string name = sqlData.GetString(sqlData.GetOrdinal("Name"));
        //     SKU sku = new SKU(sqlData.GetString(sqlData.GetOrdinal("SKU")));
        //     Barcode? barcode = BarcodeEAN13Linear.New("Test");//new Barcode(sqlData.GetString(sqlData.GetOrdinal("Barcode")), (BarcodeType)sqlData.GetInt32(sqlData.GetOrdinal("BarcodeType"))));
        //     int categoryId = sqlData.GetInt32(sqlData.GetOrdinal("CategoryId"));
        //     string imageUrl = sqlData.GetString(sqlData.GetOrdinal("ImageUrl"));
        //     bool requiresExpiryTracking = sqlData.GetBoolean(sqlData.GetOrdinal("RequiresExpiryTracking"));
        //     bool canBeReturnedIfOpened = sqlData.GetBoolean(sqlData.GetOrdinal("CanBeReturnedIfOpened"));
        //     int nearExpiryThresholdInDays = sqlData.GetInt32(sqlData.GetOrdinal("NearExpiryThresholdInDays"));

        //     UnitOfMeasure baseUnit = (UnitOfMeasure)sqlData.GetInt32(sqlData.GetOrdinal("UnitBase"));




        //     ProductPackaging packaging = new ProductPackaging(baseUnit);

        //     if (!Enum.TryParse(sqlData.GetString(sqlData.GetOrdinal("Status")), out ProductStatus status))
        //         throw new InvalidOperationException("Invalid product status value in database.");

        //     var product = new Product(productId, name, sku, categoryId, packaging, requiresExpiryTracking, canBeReturnedIfOpened, nearExpiryThresholdInDays);

        //     switch (status)
        //     {
        //         case ProductStatus.Active:
        //             product.Activate();
        //             break;
        //         case ProductStatus.Discontinued:
        //             product.Discontinue();
        //             break;
        //         default:
        //             throw new InvalidOperationException("Unhandled product status value.");
        //     }

        //     return product;
        // }
    }
}
