using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Mappers
{
    public static class ProductMapper
    {
        public static Product ToProductDomain(this SqlDataReader sqlData)
        {
            var product = new Product(
              sqlData.GetInt32(sqlData.GetOrdinal("ProductId")),
              sqlData.GetString(sqlData.GetOrdinal("Name")),
              sqlData.GetString(sqlData.GetOrdinal("SKU")),
              sqlData.GetString(sqlData.GetOrdinal("Barcode")),
              sqlData.GetString(sqlData.GetOrdinal("ImageUrl")),
              sqlData.GetInt32(sqlData.GetOrdinal("CategoryId"))
              );

            if (!sqlData.GetBoolean(sqlData.GetOrdinal("IsActive")))
                product.Deactivate();

            return product;
        }
    }
}
