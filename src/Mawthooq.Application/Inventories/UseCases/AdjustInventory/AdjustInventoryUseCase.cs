using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.Entities;
using Mawthooq.Domain.Inventories.ValueObjects;

namespace Mawthooq.Application.Inventories.UseCases.AdjustInventory
{
    public class AdjustInventoryUseCase
    {
        private readonly IInventoryRepository _inventoryRepository;
        public AdjustInventoryUseCase(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public AdjustInventoryResponse Execute(AdjustInventoryRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Inventory? inventory = _inventoryRepository.GetByInventoryKey(new InventoryKey(request.InventoryKey.ProductId, request.InventoryKey.BranchId));

            if (inventory is null)
                throw new InvalidOperationException($"Inventory for Product ID {request.InventoryKey.ProductId} and Branch ID {request.InventoryKey.BranchId} does not exist.");

            int oldQuantity = inventory.GetTotalQuantity();

            inventory.AdjustQuantity(request.NewQuantity, request.Reason, request.AdjustedCost.ToDomain(), request.AdjustmentDate);

            inventory = _inventoryRepository.UpdateAndReturn(inventory);

            int newQuantity = inventory.GetTotalQuantity();

            int difference = newQuantity - oldQuantity;


            return new AdjustInventoryResponse(
                request.InventoryKey,
                newQuantity,
                difference
            );
        }
    }
}
