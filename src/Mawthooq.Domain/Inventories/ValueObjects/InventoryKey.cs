using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Inventories.ValueObjects
{
    public record class InventoryKey (int ProductId, int BranchId);
}
