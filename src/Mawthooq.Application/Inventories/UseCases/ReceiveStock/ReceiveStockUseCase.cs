using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Branches;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.Entities;
using Mawthooq.Domain.Inventories.Enums;
using Mawthooq.Domain.Inventories.ValueObjects;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Inventories.UseCases.ReceiveStock
{
    public class ReceiveStockUseCase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBranchRepository _branchRepository;

        public ReceiveStockUseCase(IInventoryRepository inventoryRepository, IProductRepository productRepository, IBranchRepository branchRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _branchRepository = branchRepository;
        }

        public ReceiveStockResponse Execute(ReceiveStockRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Product? product = _productRepository.GetById(request.ProductId);
            Branch? branch = _branchRepository.GetById(request.BranchId);

            if (product is null)
                throw new InvalidOperationException($"Product with ID {request.ProductId} does not exist.");

            if (branch is null)
                throw new InvalidOperationException($"Branch with ID {request.BranchId} does not exist.");

            Inventory? inventory = _inventoryRepository.GetByInventoryKey(new InventoryKey(request.ProductId, request.BranchId));

            if (inventory is null)
            {
                Inventory newinventory = Inventory.Create(request.ProductId, request.BranchId, request.SellingPrice.ToDomain());
                newinventory.ReceiveStock(request.Quantity, request.PurchaseCost.ToDomain(), request.ReceivedDate, request.ExpiryDate, BatchSource.Purchase);
                inventory = _inventoryRepository.Add(newinventory);
            }
            else
            {
                inventory.ReceiveStock(request.Quantity, request.PurchaseCost.ToDomain(), request.ReceivedDate, request.ExpiryDate, BatchSource.InventoryAdjustment);
                inventory = _inventoryRepository.UpdateAndReturn(inventory);
            }

            if (inventory is null)
                throw new InvalidOperationException("Failed to receive stock.");

            Batch? lastBatch = inventory.Batches.Last();

            return new ReceiveStockResponse(
                new DTOs.InventoryKeyDto(inventory.ProductId, inventory.BranchId),
                lastBatch.BatchId,
                lastBatch.Quantity,
                inventory.SellingPrice.ToDto(),
                lastBatch.PurchaseCost.ToDto(),
                lastBatch.ReceivedDate,
                lastBatch.ExpiryDate
                );
        }
    }
}
