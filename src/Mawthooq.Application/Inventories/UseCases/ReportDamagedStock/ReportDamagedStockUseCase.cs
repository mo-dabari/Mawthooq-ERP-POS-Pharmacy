using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.Mappers;
using Mawthooq.Domain.Inventories;

namespace Mawthooq.Application.Inventories.UseCases.ReportDamagedStock
{
    public class ReportDamagedStockUseCase
    {
        private readonly IInventoryRepository InventoryRepository;
        public ReportDamagedStockUseCase(IInventoryRepository inventoryRepository)
        {
            InventoryRepository = inventoryRepository;
        }

        public ReportDamagedResponse Execute(ReportDamagedRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            Inventory? inventory = InventoryRepository.GetById(request.InventoryKey.ToDomain());

            if (inventory is null)
                throw new InvalidOperationException("Inventory not found.");

            var damagedBatch = inventory.ReportDamagedStock(request.BatchId, request.Quantity, request.Reason);


            Inventory updatedInventory = InventoryRepository.UpdateAndReturn(inventory);

            return new ReportDamagedResponse(
                updatedInventory.DamagedBatchValues.Last().DamagedBatchId,
                damagedBatch.Quantity,
                damagedBatch.Reason.ToString(),
                damagedBatch.DamagedAt
            );
        }

    }
}
