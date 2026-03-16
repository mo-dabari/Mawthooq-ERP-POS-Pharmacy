using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Inventories.DTOs
{
    public record class InventoryKeyDto(int ProductId,int BranchId)
    {
    }
}
