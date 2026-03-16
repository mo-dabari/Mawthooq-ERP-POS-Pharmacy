using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.DTOs;
using Mawthooq.Domain.Inventories.Enums;

namespace Mawthooq.Application.Inventories.UseCases.ReportDamagedStock
{
    public record class ReportDamagedRequest(
        InventoryKeyDto InventoryKey,
        int BatchId,
        int Quantity,
        DamagedReason Reason
    )
    {
    }
}
