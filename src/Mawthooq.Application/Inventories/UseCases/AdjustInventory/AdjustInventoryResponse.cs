using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.DTOs;
using Mawthooq.Domain.Inventories.Enums;

namespace Mawthooq.Application.Inventories.UseCases.AdjustInventory
{
    public record class AdjustInventoryResponse(
        InventoryKeyDto InventoryKey,
        int NewQuantity,
        int QuantityDifference
    );
}
