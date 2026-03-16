using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Money;

namespace Mawthooq.Domain.Inventories.Entities
{
    public class Batch
    {
        public int BatchId { get; private set; }
        public int Quantity { get; private set; }
        public Money PurchaseCost { get; private set; }
        public DateTime ReceivedDate { get; private set; }
        public DateOnly ExpiryDate { get; private set; }

        public Batch(int batchId, int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate)
        : this(quantity, purchaseCost, receivedDate, expiryDate)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchId, nameof(batchId));
            BatchId = batchId;
        }

        private Batch(int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            ArgumentNullException.ThrowIfNull(purchaseCost, nameof(purchaseCost));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(purchaseCost.Amount, nameof(purchaseCost.Amount));

            if (receivedDate > DateTime.UtcNow)
                throw new ArgumentOutOfRangeException(nameof(receivedDate), "The delivery date cannot be in the future.");

            if (expiryDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentOutOfRangeException(nameof(expiryDate), "The Batch is already expired!.");

            Quantity = quantity;
            PurchaseCost = purchaseCost;
            ReceivedDate = receivedDate;
            ExpiryDate = expiryDate;
        }
        internal static Batch Create(int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate)
        {
            return new Batch(quantity, purchaseCost, receivedDate, expiryDate);
        }

        internal void IncreaseQuantity(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            Quantity += quantity;
        }

        internal void DecreaseQuantity(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            if (Quantity < quantity)
                throw new InvalidOperationException("Insufficient batch quantity");
            Quantity -= quantity;
        }

        public void SetBatchId(int batchId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchId, nameof(batchId));
            if (BatchId != 0)
                throw new InvalidOperationException("Batch ID is already set and cannot be changed.");
            BatchId = batchId;
        }
    }
}
