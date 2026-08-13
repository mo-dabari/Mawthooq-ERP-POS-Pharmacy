using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.DTOs;
using Mawthooq.Domain.Inventories.ValueObjects;

namespace Mawthooq.Application.Inventories.Mappers
{
    public static class InventoryKeyMapper
    {

        public static InventoryKeyDto ToDto(this InventoryKey inventoryKey)
        {
            ArgumentNullException.ThrowIfNull(inventoryKey, nameof(inventoryKey));

            return new InventoryKeyDto(inventoryKey.ProductId, inventoryKey.BranchId);
        }

        public static InventoryKey ToDomain(this InventoryKeyDto inventoryKeyDto)
        {
            ArgumentNullException.ThrowIfNull(inventoryKeyDto, nameof(inventoryKeyDto));

            return new InventoryKey(inventoryKeyDto.ProductId, inventoryKeyDto.BranchId);
        }
    }
}
