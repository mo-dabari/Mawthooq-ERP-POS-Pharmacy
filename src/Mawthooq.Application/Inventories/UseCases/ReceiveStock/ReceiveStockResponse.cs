using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Inventories.DTOs;

namespace Mawthooq.Application.Inventories.UseCases.ReceiveStock
{
    public record class ReceiveStockResponse(
        InventoryKeyDto InventoryKey,
        int BatchId,
        int QuantityReceived,
        MoneyDto SellingPrice,
        MoneyDto PurchaseCost,
        DateTime ReceivedDate,
        DateOnly ExpiryDate
         )
    {

    }
}
