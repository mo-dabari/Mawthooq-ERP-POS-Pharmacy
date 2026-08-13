using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Inventories.UseCases.ReceiveStock
{
    public record class ReceiveStockRequest(
        int ProductId,
        int BranchId,
        int Quantity,
        MoneyDto PurchaseCost,
        MoneyDto SellingPrice,
        DateTime ReceivedDate,
        DateOnly ExpiryDate
    )
    {
    }
}
