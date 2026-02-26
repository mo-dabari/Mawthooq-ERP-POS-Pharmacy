using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Inventory.ValueObjects
{
    public record class InventoryKey (int ProductId, int BranchId);
}
