using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.DTOs;
using Mawthooq.Domain.SupplierReturns.Enums;

namespace Mawthooq.Application.Inventories.UseCases.ReturnStockToSupplier.DTOs
{
    public record class SupplierReturnDto(
        int SupplierReturnId,
        int SupplierId,
        InventoryKeyDto InventoryKey,
        int BatchId,
        int Quantity,
        SupplierReturnReason Reason,
        DateTime ReturnedAt,
        ReturnStatus Status
    )
    {

    }
}
