using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.Entities;
using Mawthooq.Domain.Inventories.Enums;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Mappers
{
    public static class BatchMapper
    {
        public static Batch MapToBatch(SqlDataReader record)
        {
            int batchId = record.GetInt32(record.GetOrdinal("BatchId"));
            int quantity = record.GetInt32(record.GetOrdinal("Quantity"));
            decimal purchaseCostAmount = record.GetDecimal(record.GetOrdinal("PurchaseCost"));
            DateTime receivedDate = record.GetDateTime(record.GetOrdinal("ReceivedDate"));
            DateOnly expiryDate = DateOnly.FromDateTime(record.GetDateTime(record.GetOrdinal("ExpiryDate")));
            BatchSource source = Enum.Parse<BatchSource>(record.GetString(record.GetOrdinal("Source")));


            if (!Enum.TryParse(record.GetString(record.GetOrdinal("CurrencyCode")), out Currency currency))
                throw new InvalidOperationException("Invalid currency code in database.");

            Money purchaseCost = new(purchaseCostAmount, currency);

            return new Batch(batchId, quantity, purchaseCost, receivedDate, expiryDate, source);
        }
    }
}
