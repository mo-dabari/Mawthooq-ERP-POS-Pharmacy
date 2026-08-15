using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.Entities;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Mappers
{
    public static class InventoryMapper
    {
        public static Inventory MapToInventory(SqlDataReader record, IEnumerable<Batch>? batches = null)
        {
            int inventoryId = record.GetInt32(record.GetOrdinal("InventoryId"));
            int productId = record.GetInt32(record.GetOrdinal("ProductId"));
            int branchId = record.GetInt32(record.GetOrdinal("BranchId"));
            decimal sellingPriceAmount = record.GetDecimal(record.GetOrdinal("SellingPrice"));

            if (!Enum.TryParse(record.GetString(record.GetOrdinal("CurrencyCode")), out Currency currency))
                throw new InvalidOperationException("Invalid currency code in database.");

            Money sellingPrice = new(sellingPriceAmount, currency);

            return new Inventory (inventoryId, productId, branchId, sellingPrice);
        }
    }
}
