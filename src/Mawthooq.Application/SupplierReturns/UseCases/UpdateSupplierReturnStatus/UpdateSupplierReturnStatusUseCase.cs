using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.SupplierReturns.Mappers;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.Entities;
using Mawthooq.Domain.SupplierReturns;
using Mawthooq.Domain.SupplierReturns.Enums;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.SupplierReturns.UseCases.UpdateSupplierReturnStatus
{
    public class UpdateSupplierReturnStatusUseCase
    {
        private readonly ISupplierReturnRepository _supplierReturnRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISupplierRepository _supplierRepository;

        public UpdateSupplierReturnStatusUseCase(ISupplierReturnRepository supplierReturnRepository, IInventoryRepository inventoryRepository, ISupplierRepository supplierRepository)
        {
            _supplierReturnRepository = supplierReturnRepository;
            _inventoryRepository = inventoryRepository;
            _supplierRepository = supplierRepository;
        }

        public UpdateSupplierReturnStatusResponse Execute(UpdateSupplierReturnStatusRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            SupplierReturn? supplierReturn = _supplierReturnRepository.GetById(request.SupplierReturnId);

            MoneyDto? totalReturnAmount = default;

            if (supplierReturn == null)
                throw new InvalidOperationException($"Supplier return with ID {request.SupplierReturnId} not found.");

            supplierReturn.SetReturnStatus(request.NewStatus);

            if (supplierReturn.Status == ReturnStatus.Approved)
            {
                Supplier? supplier = _supplierRepository.GetById(supplierReturn.SupplierId);
                if (supplier == null)
                    throw new InvalidOperationException($"Supplier with ID {supplierReturn.SupplierId} not found.");

                Inventory? inventory = _inventoryRepository.GetByInventoryKey(supplierReturn.InventoryKey);
                if (inventory == null)
                    throw new InvalidOperationException($"Inventory with key {supplierReturn.InventoryKey} not found.");

                Batch? batch = inventory.Batches.FirstOrDefault(b => b.BatchId == supplierReturn.BatchId);
                if (batch == null)
                    throw new InvalidOperationException($"Batch with ID {supplierReturn.BatchId} not found in inventory with key {supplierReturn.InventoryKey}.");

                if (batch.Quantity < supplierReturn.Quantity)
                    throw new InvalidOperationException($"Not enough quantity in batch {supplierReturn.BatchId} to return. Available quantity: {batch.Quantity}, requested return quantity: {supplierReturn.Quantity}.");

                totalReturnAmount = new(batch.PurchaseCost.Amount * supplierReturn.Quantity, batch.PurchaseCost.Currency);

                supplier.DeductBalance(totalReturnAmount.ToDomain());

                // inventory.ReturnStockToBatch(supplierReturn.BatchId, supplierReturn.Quantity);

                _supplierReturnRepository.Update(supplierReturn);
                _supplierRepository.Update(supplier);
                _inventoryRepository.Update(inventory);
            }
            return new UpdateSupplierReturnStatusResponse(
                supplierReturn.ToDto(),
                totalReturnAmount
            );
        }
    }
}
