using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.Mappers;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.Entities;
using Mawthooq.Domain.SupplierReturns;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.SupplierReturns.UseCases.ReturnStockToSupplier
{
    public class ReturnStockToSupplierUseCase
    {
        private readonly ISupplierReturnRepository _supplierReturnRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISupplierRepository _supplierRepository;

        public ReturnStockToSupplierUseCase(ISupplierReturnRepository supplierReturnRepository, IInventoryRepository inventoryRepository, ISupplierRepository supplierRepository)
        {
            _supplierReturnRepository = supplierReturnRepository;
            _inventoryRepository = inventoryRepository;
            _supplierRepository = supplierRepository;
        }

        public ReturnStockToSupplierResponse Execute(ReturnStockToSupplierRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Inventory? inventory = _inventoryRepository.GetById(request.InventoryKey.ToDomain());
            if (inventory is null)
                throw new InvalidOperationException($"Inventory with key {request.InventoryKey} does not exist.");

            Supplier? supplier = _supplierRepository.GetById(request.SupplierId);
            if (supplier is null)
                throw new InvalidOperationException($"Supplier with ID {request.SupplierId} does not exist.");

                Batch? batch = inventory.Batches.FirstOrDefault(b => b.BatchId == request.BatchId);
            if (batch is null)
                throw new InvalidOperationException($"Batch with ID {request.BatchId} does not exist in inventory with key {request.InventoryKey}.");

            SupplierReturn supplierReturn = SupplierReturn.Create(request.SupplierId, request.InventoryKey.ToDomain(), request.BatchId, request.Quantity, request.Reason);
            SupplierReturn? savedSupplierReturn = _supplierReturnRepository.Add(supplierReturn);

            if (savedSupplierReturn is null)
                throw new InvalidOperationException("Failed to create supplier return");

            return new ReturnStockToSupplierResponse(
                savedSupplierReturn.SupplierReturnId,
                savedSupplierReturn.SupplierId,
                savedSupplierReturn.InventoryKey.ToDto(),
                savedSupplierReturn.BatchId,
                savedSupplierReturn.Quantity,
                savedSupplierReturn.Reason,
                savedSupplierReturn.ReturnedAt,
                savedSupplierReturn.Status
            );
        }
    }
}
