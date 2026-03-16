using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.Enums;
using Mawthooq.Domain.Inventories.ValueObjects;
using Mawthooq.Domain.SupplierReturns.Enums;

namespace Mawthooq.Domain.SupplierReturns
{
    public class SupplierReturn
    {
        public int SupplierReturnId { get; private set; }
        public int SupplierId { get; }
        public InventoryKey InventoryKey { get; }
        public int BatchId { get; private set; }
        public int Quantity { get; private set; }
        public SupplierReturnReason Reason { get; }
        public DateTime ReturnedAt { get; }
        public ReturnStatus Status { get; private set; }

        public SupplierReturn(int supplierReturnId, int supplierId, InventoryKey inventoryKey, int batchId, int quantity, SupplierReturnReason reason, DateTime returnedAt, ReturnStatus status)
        : this(supplierId, inventoryKey, batchId, quantity, reason)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(supplierReturnId, nameof(supplierReturnId));
            SupplierReturnId = supplierReturnId;
        }
        private SupplierReturn(int supplierId, InventoryKey inventoryKey, int batchId, int quantity, SupplierReturnReason reason)
        {
            ArgumentNullException.ThrowIfNull(inventoryKey, nameof(inventoryKey));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(supplierId, nameof(supplierId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchId, nameof(batchId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));

            if (!Enum.IsDefined(reason))
                throw new InvalidEnumArgumentException(nameof(reason), (int)reason, typeof(SupplierReturnReason));

            SupplierId = supplierId;
            InventoryKey = inventoryKey;
            BatchId = batchId;
            Quantity = quantity;
            Reason = reason;
            ReturnedAt = DateTime.UtcNow;
            Status = ReturnStatus.Pending;
        }
        public static SupplierReturn Create(int supplierId, InventoryKey inventoryKey, int batchId, int quantity, SupplierReturnReason reason)
        {
            return new SupplierReturn(supplierId, inventoryKey, batchId, quantity, reason);
        }

        public void SetSuppierReturnId(int supplierReturnId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(supplierReturnId, nameof(supplierReturnId));
            SupplierReturnId = supplierReturnId;
        }
        public void SetReturnStatus(ReturnStatus newStatus)
        {
            if (Status == newStatus)
                throw new InvalidOperationException($"Supplier return with ID is already in status {newStatus}.");

            if (Status == ReturnStatus.Approved && newStatus == ReturnStatus.Rejected)
                throw new InvalidOperationException($"Cannot change status of supplier return with ID" +
                    $" from {Status} to {newStatus}.");

            if (Status == ReturnStatus.Rejected && newStatus == ReturnStatus.Approved)
                throw new InvalidOperationException($"Cannot change status of supplier return with ID " +
                    $"from {Status} to {newStatus}.");

            Status = newStatus;
        }
    }
}
