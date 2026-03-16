using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Common.Interfaces;
using Mawthooq.Domain.Inventories.ValueObjects;

namespace Mawthooq.Domain.Inventories
{
    public interface IInventoryRepository : IBaseRepository<Inventory, InventoryKey>
    {
        Inventory? GetInventoryByProductAndBranch(int productId, int branchId);
        Inventory UpdateAndReturn(Inventory inventory);
    }
}
