using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Sales.Entity;
using Mawthooq.Domain.Sales.Enums;
using Mawthooq.Domain.ValueObjects.Money;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Domain.Sales
{
    public class Sale
    {
        public int SaleId { get; }
        public int BranchId { get; }
        public Currency Currency { get; }
        public DateTime SaleDate { get; }
        public SaleStatus Status { get; private set; }
        private List<SaleLine> _lines = new();
        public IReadOnlyList<SaleLine> Lines => _lines.AsReadOnly();
        public Money DiscountAmount { get; private set; }
        public Money FinalAmount => TotalAmount - DiscountAmount;
        public Money TotalAmount => _lines.Aggregate(
            Money.Zero(Currency),
            (sum, line) => sum + line.TotalAmount);
        public Money TotalPurchaseCost => _lines.Aggregate(
            Money.Zero(Currency),
            (sum, line) => sum + line.TotalPurchaseCost);
        public Money TotalGrossProfit => _lines.Aggregate(
        Money.Zero(Currency),
        (sum, line) => sum + line.GrossProfit);


        public Sale(int saleId, int branchId, Currency currency)
        : this(branchId, currency)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(saleId, nameof(saleId));
            SaleId = saleId;
        }

        private Sale(int branchId, Currency currency)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(branchId, nameof(branchId));

            if (!Enum.IsDefined(typeof(Currency), currency))
                throw new InvalidEnumArgumentException(nameof(currency), (int)currency, typeof(Currency));

            BranchId = branchId;
            Currency = currency;
            SaleDate = DateTime.UtcNow;
            Status = SaleStatus.Open;
            DiscountAmount = Money.Zero(currency);
        }

        public static Sale Create(int branchId, Currency currency)
        {
            return new Sale(branchId, currency);
        }

        public void AddLine(int productId, int quantity, Money sellingPrice, Money purchaseCost)
        {
            if (Status != SaleStatus.Open)
                throw new InvalidOperationException("Cannot modify a confirmed or cancelled sale.");

            if (sellingPrice.Currency != Currency)
                throw new InvalidOperationException("Currency mismatch.");

            if (purchaseCost.Currency != Currency)
                throw new InvalidOperationException("Purchase cost currency must match sale currency.");

            SaleLine? existing = _lines.FirstOrDefault(
                l => l.ProductId == productId &&
                l.SellingPrice == sellingPrice);

            if (existing is not null)
            {
                existing.IncreaseQuantity(quantity);
                return;
            }
            _lines.Add(SaleLine.Create(
                productId: productId,
                quantity: quantity,
                sellingPrice: sellingPrice,
                purchaseCost: purchaseCost));
        }

        public void RemoveLine(int saleLineId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(saleLineId);

            if (Status != SaleStatus.Open)
                throw new InvalidOperationException("Cannot modify a confirmed or canceled sale");

            SaleLine? existing = _lines.FirstOrDefault(l => l.SaleLineId == saleLineId);

            if (existing is null)
                throw new InvalidOperationException("Sale line not found.");

            _lines.Remove(existing);
        }

        public void Confirm()
        {
            if (Status != SaleStatus.Open)
                throw new InvalidOperationException("Sale is already confirmed or cancelled.");

            if (!_lines.Any())
                throw new InvalidOperationException("Cannot confirm a sale with no lines.");

            Status = SaleStatus.Confirmed;
        }

        public void Cancel()
        {
            if (Status == SaleStatus.Cancelled)
                throw new InvalidOperationException("Sale is already cancelled.");

            Status = SaleStatus.Cancelled;
        }

        public void ApplyInvoiceDiscount(Money discountAmount)
        {
            ArgumentNullException.ThrowIfNull(discountAmount, nameof(discountAmount));

            if (Status != SaleStatus.Open)
                throw new InvalidOperationException("Cannot modify a confirmed or cancelled sale.");

            if (discountAmount.Currency != Currency)
                throw new InvalidOperationException("Currency mismatch.");

            if (discountAmount.Amount > TotalAmount.Amount)
                throw new InvalidOperationException("Discount amount cannot exceed total sale amount.");

            DiscountAmount = discountAmount;
        }
    }
}
