using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.ValueObjects;
using Mawthooq.Domain.Inventories.Entities;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Mawthooq.Domain.Inventories.Enums;
using System.ComponentModel;

namespace Mawthooq.Domain.Inventories
{
    public class Inventory
    {
        public int InventoryId { get; }

        public int ProductId { get; }
        public int BranchId { get; }

        private readonly List<Batch> _batches = new();

        public IReadOnlyList<Batch> Batches => _batches;
        public Money SellingPrice { get; private set; }

        public Inventory(int inventoryId, int productId, int branchId, Money sellingPrice)
        : this(productId, branchId, sellingPrice)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inventoryId, nameof(inventoryId));
            InventoryId = inventoryId;
        }

        private Inventory(int productId, int branchId, Money sellingPrice)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(branchId, nameof(branchId));
            ArgumentNullException.ThrowIfNull(sellingPrice, nameof(sellingPrice));

            ProductId = productId;
            BranchId = branchId;
            SellingPrice = sellingPrice;
        }

        public static Inventory Create(int productId, int branchId, Money sellingPrice)
        {
            return new Inventory(productId, branchId, sellingPrice);
        }

        private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

        private IEnumerable<Batch> SelectFefoBatches()
        {
            return _batches
                    .Where(b => !b.IsExpired && b.Quantity > 0)
                    .OrderBy(b => b.ExpiryDate)
                    .ThenBy(b => b.ReceivedDate);
        }

        public int GetAvailableQuantity()
        {
            return _batches
            .Where(b => !b.IsExpired && b.Quantity > 0)
            .Sum(b => b.Quantity);
        }

        private List<BatchAllocation> SelectFefoBatches(int requiredQuantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(requiredQuantity, nameof(requiredQuantity));

            if (GetAvailableQuantity() < requiredQuantity)
                throw new InvalidOperationException("Insufficient stock.");

            List<BatchAllocation> allocations = new();

            int remaining = requiredQuantity;

            foreach (var batch in SelectFefoBatches())
            {
                if (remaining == 0)
                    break;

                int allocated = Math.Min(batch.Quantity, remaining);

                allocations.Add(new BatchAllocation(batch, allocated));

                remaining -= allocated;
            }

            return allocations;
        }



        public int GetTotalQuantity() => _batches.Sum(b => b.Quantity);



        public void ReceiveStock(int quantity, Money purchaseCost, DateTime receivedDate, DateOnly expiryDate, BatchSource source)
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

            Batch batch = Batch.Create(quantity, purchaseCost, receivedDate, expiryDate, source);

            InsertBatchOrdered(batch);
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
                Batch batch = SelectFefoBatches(remainingToDeduct).First().Batch;

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

        public void AdjustQuantity(int newQuantity, AdjustmentReason reason, Money adjustmentCost, DateOnly adjustmentDateExpiry)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(newQuantity, nameof(newQuantity));

            if (!Enum.IsDefined(reason))
                throw new InvalidEnumArgumentException(nameof(reason), (int)reason, typeof(AdjustmentReason));

            int currentQuantity = GetTotalQuantity();

            if (currentQuantity == newQuantity)
                return;



            // _adjustmentLogs.Add(AdjustmentLog.Create(currentQuantity, newQuantity, reason));



            int difference = newQuantity - currentQuantity;

            if (difference > 0)
            {
                Batch adjustmentBatch = Batch.Create(difference, adjustmentCost, DateTime.UtcNow, adjustmentDateExpiry, BatchSource.InventoryAdjustment);
                InsertBatchOrdered(adjustmentBatch);
            }
            else
                DeductStock(-difference);
        }

        // public DamagedBatch ReportDamagedStock(int batchId, int quantity, DamagedReason reason)
        // {
        //     ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));

        //     if (!Enum.IsDefined(reason))
        //         throw new InvalidEnumArgumentException(nameof(reason), (int)reason, typeof(DamagedReason));

        //     Batch? batch = _batches.FirstOrDefault(b => b.BatchId == batchId);

        //     if (batch is null)
        //         throw new InvalidOperationException("Batch not found.");

        //     if (batch.Quantity < quantity)
        //         throw new InvalidOperationException("Insufficient batch quantity to report as damaged.");

        //     batch.DecreaseQuantity(quantity);

        //     _damagedBatches.Add(DamagedBatch.Create(quantity, reason));

        //     if (batch.Quantity == 0)
        //         _batches.Remove(batch);

        //     return DamagedBatchValues.Last();
        // }

        public void ChangeSellingPrice(Money newPrice)
        {
            ArgumentNullException.ThrowIfNull(newPrice, nameof(newPrice));

            if (newPrice.Currency != SellingPrice.Currency)
                throw new InvalidOperationException("Cannot change price to a different currency.");

            if (newPrice.Amount == 0)
                throw new InvalidOperationException("Selling price must be greater than zero.");

            SellingPrice = newPrice;
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
            b.PurchaseCost == newBatch.PurchaseCost &&
            b.Source == newBatch.Source);

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
        private void PurgeEmptyBatches(List<Batch> emptiedBatches)
        {
            foreach (Batch batch in emptiedBatches)
                _batches.Remove(batch);
        }

    }
}
