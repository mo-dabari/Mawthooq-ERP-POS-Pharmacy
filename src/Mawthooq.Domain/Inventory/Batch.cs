using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Money;

namespace Mawthooq.Domain.Inventory
{
    public class Batch
    {
        public int BatchId { get; private set; }
        public int Quantity { get; private set; }
        public Money PurchaseCost { get; private set; }
        public DateTime ReceivedDate { get; private set; }
        public DateOnly ExpiryDate { get; private set; }

        public Batch(int batchId, int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchId, nameof(batchId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            ArgumentNullException.ThrowIfNull(purchaseCost, nameof(purchaseCost));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(purchaseCost.Amount, nameof(purchaseCost.Amount));

            if (receivedDate > DateTime.UtcNow)
                throw new ArgumentOutOfRangeException(nameof(receivedDate), "The delivery date cannot be in the future.");

            if (expiryDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentOutOfRangeException(nameof(expiryDate), "The Batch is already expired!.");

            BatchId = batchId;
            Quantity = quantity;
            PurchaseCost = purchaseCost;
            ReceivedDate = receivedDate;
            ExpiryDate = expiryDate;
        }

        public void IncreaseQuantity(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            Quantity += quantity;
        }

        public void DecreaseQuantity(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            if (Quantity < quantity)
                throw new InvalidOperationException("Insufficient batch quantity");
            Quantity -= quantity;
        }
    }
}
