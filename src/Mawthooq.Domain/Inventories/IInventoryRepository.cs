using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.ValueObjects;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.Inventories
{
    public interface IInventoryRepository : IBaseRepository<Inventory, InventoryKey>
    {
        IEnumerable<Inventory> Get_All_Available_Stock_For_Each_Branch();
        Inventory? GetByInventoryKey(InventoryKey inventoryKey);
        Inventory UpdateAndReturn(Inventory inventory);
    }
}
