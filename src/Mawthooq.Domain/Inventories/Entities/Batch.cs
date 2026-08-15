using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Inventories.Entities
{
    public class Batch
    {
        public int BatchId { get; }
        public int Quantity { get; private set; }

        public Money PurchaseCost { get; }
        public DateTime ReceivedDate { get; }
        public DateOnly ExpiryDate { get; }
        public BatchSource Source { get; }

        public bool IsExpired => ExpiryDate <= DateOnly.FromDateTime(DateTime.UtcNow);


        public Batch(int batchId, int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate, BatchSource source)
        : this(quantity, purchaseCost, receivedDate, expiryDate, source)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchId, nameof(batchId));
            BatchId = batchId;
        }

        private Batch(int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate, BatchSource source)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            ArgumentNullException.ThrowIfNull(purchaseCost, nameof(purchaseCost));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(purchaseCost.Amount, nameof(purchaseCost.Amount));

            if (receivedDate > DateTime.UtcNow)
                throw new ArgumentOutOfRangeException(nameof(receivedDate), "The delivery date cannot be in the future.");

            if (expiryDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentOutOfRangeException(nameof(expiryDate), "The Batch is already expired!.");

            if (!Enum.IsDefined(typeof(BatchSource), source))
                throw new InvalidOperationException("Invalid batch source.");

            Quantity = quantity;
            PurchaseCost = purchaseCost;
            ReceivedDate = receivedDate;
            ExpiryDate = expiryDate;
            Source = source;
        }
        internal static Batch Create(int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate, BatchSource source)
        {
            return new Batch(quantity, purchaseCost, receivedDate, expiryDate, source);
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
    }
}
 // Batch
        // public void SetOutOfStock()
        // {
        //     if (Status.Equals(ProductStatus.OutOfStock))
        //         return;
        //     Status = ProductStatus.OutOfStock;
        // }

        // Batch
        // public bool IsNearExpiry(DateOnly expiryDate)
        // {
        //     if (!RequiresExpiryTracking)
        //         throw new InvalidOperationException("This product does not require expiry tracking.");

        //     return (expiryDate.Day - DateOnly.FromDateTime(DateTime.UtcNow).Day) <= NearExpiryThresholdInDays;
        // }
