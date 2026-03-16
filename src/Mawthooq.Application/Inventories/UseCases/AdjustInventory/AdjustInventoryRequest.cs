using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.DTOs;
using Mawthooq.Domain.Inventories.Enums;

namespace Mawthooq.Application.Inventories.UseCases.AdjustInventory
{
    public record class AdjustInventoryRequest(
        InventoryKeyDto InventoryKey,
        int NewQuantity,
        AdjustmentReason Reason
    )
    {
    }
}
