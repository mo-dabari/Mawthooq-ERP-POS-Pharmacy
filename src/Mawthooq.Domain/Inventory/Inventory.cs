using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventory.ValueObjects;
using Mawthooq.Domain.ValueObjects.Money;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Domain.Inventory
{
    public class Inventory
    {
        public InventoryKey InventoryId { get; }
        public int ProductId { get; }
        public Money SellingPrice { get; private set; }
        public int BranchId { get; }
        private List<Batch> _batches;
        public IReadOnlyList<Batch> BatchValues => _batches.AsReadOnly();

        public Inventory(int productId, int branchId, Money sellingPrice)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(branchId, nameof(branchId));
            ArgumentNullException.ThrowIfNull(sellingPrice, nameof(sellingPrice));

            InventoryId = new(productId, branchId);
            ProductId = productId;
            BranchId = branchId;
            SellingPrice = sellingPrice;
            _batches = new();
        }

        private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

        public int GetAvailableQuantity()
        {
            return _batches
            .Where(b => b.ExpiryDate > Today)
            .Sum(b => b.Quantity);
        }

        private Batch GetTheOldestBatch()
        {
            return _batches.FirstOrDefault(b => b.ExpiryDate > Today)
        ?? throw new InvalidOperationException("No valid batches available.");
        }

        public void ReceiveStock(int batchId, int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate)
        {
            ArgumentNullException.ThrowIfNull(purchaseCost, nameof(purchaseCost));

            if (SellingPrice.Currency != purchaseCost.Currency)
                throw new InvalidOperationException("Batch currency must match inventory currency.");

            Batch batch = new(batchId, quantity, purchaseCost, receivedDate, expiryDate);

            InsertBatchOrdered(batch);
        }

        private void InsertBatchOrdered(Batch newBatch)
        {
            if (_batches.Count == 0)
            {
                _batches.Add(newBatch);
                return;
            }

            Batch? existing = _batches.FirstOrDefault(b =>
            b.ExpiryDate == newBatch.ExpiryDate &&
            b.ReceivedDate == newBatch.ReceivedDate &&
            b.PurchaseCost == newBatch.PurchaseCost);

            if (existing is not null)
            {
                existing.IncreaseQuantity(newBatch.Quantity);
                return;
            }

            for (int i = 0; i < _batches.Count; i++)
            {
                if (newBatch.ExpiryDate < _batches[i].ExpiryDate)
                {
                    _batches.Insert(i, newBatch);
                    return;
                }

                if (newBatch.ExpiryDate == _batches[i].ExpiryDate &&
                    newBatch.ReceivedDate < _batches[i].ReceivedDate)
                {
                    _batches.Insert(i, newBatch);
                    return;
                }
            }

            _batches.Add(newBatch);
        }

        public DeductionResult DeductStock(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));


            if (GetAvailableQuantity() < quantity)
                throw new InvalidOperationException("Inadequate stock levels.");


            Money totalCost = Money.Zero(SellingPrice.Currency);

            int remainingToDeduct = quantity;

            List<Batch> emptiedBatches = new List<Batch>();

            while (remainingToDeduct > 0)
            {
                Batch batch = GetTheOldestBatch();

                int canTake = Math.Min(batch.Quantity, remainingToDeduct);

                totalCost += batch.PurchaseCost.Multiply(canTake);

                batch.DecreaseQuantity(canTake);

                remainingToDeduct -= canTake;

                if (batch.Quantity == 0)
                    emptiedBatches.Add(batch);
            }

            PurgeEmptyBatches(emptiedBatches);

            return new DeductionResult(totalCost, SellingPrice, quantity);
        }

        private void PurgeEmptyBatches(List<Batch> emptiedBatches)
        {
            foreach (Batch batch in emptiedBatches)
                _batches.Remove(batch);
        }

        public void ChangeSellingPrice(Money newPrice)
        {
            ArgumentNullException.ThrowIfNull(newPrice, nameof(newPrice));

            if (newPrice.Currency != SellingPrice.Currency)
                throw new InvalidOperationException("Cannot change price to a different currency.");

            if (newPrice.Amount == 0)
                throw new InvalidOperationException("Selling price must be greater than zero.");

            SellingPrice = newPrice;
        }
    }
}
