using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.Entities;

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

            Inventory? inventory = _inventoryRepository.GetInventoryByProductAndBranch(request.InventoryKey.ProductId, request.InventoryKey.BranchId);

            if (inventory is null)
                throw new InvalidOperationException($"Inventory for Product ID {request.InventoryKey.ProductId} and Branch ID {request.InventoryKey.BranchId} does not exist.");

            int oldQuantity = inventory.GetTotalQuantity();

            inventory.AdjustQuantity(request.NewQuantity, request.Reason);

            inventory = _inventoryRepository.UpdateAndReturn(inventory);

            int newQuantity = inventory.GetTotalQuantity();

            int difference = newQuantity - oldQuantity;

            AdjustmentLog lastAdjustment = inventory.AdjustmentLogs.Last();

            return new AdjustInventoryResponse(
                lastAdjustment.AdjustmentId,
                request.InventoryKey,
                newQuantity,
                difference,
                lastAdjustment.Reason
            );
        }
    }
}
