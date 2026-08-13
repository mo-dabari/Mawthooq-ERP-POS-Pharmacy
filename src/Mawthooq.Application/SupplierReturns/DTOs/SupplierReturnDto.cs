using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.DTOs;
using Mawthooq.Domain.SupplierReturns.Enums;

namespace Mawthooq.Application.SupplierReturns.DTOs
{
    public record class SupplierReturnDto(
        int SupplierReturnId,
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
