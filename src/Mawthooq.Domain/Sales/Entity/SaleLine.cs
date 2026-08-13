using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Money;

namespace Mawthooq.Domain.Sales.Entity
{
    public class SaleLine
    {
        public int SaleLineId { get; }
        public int ProductId { get; }
        public int Quantity { get; private set; }
        public Money SellingPrice { get; }
        public Money PurchaseCost { get; }
        public Money DiscountAmount { get; private set; }
        public Money FinalPrice => TotalAmount - DiscountAmount;
        public Money TotalAmount => SellingPrice.Multiply(Quantity);
        public Money TotalPurchaseCost => PurchaseCost.Multiply(Quantity);
        public Money GrossProfit => TotalAmount - TotalPurchaseCost;

        public SaleLine(int saleLineId, int productId, int quantity, Money sellingPrice, Money purchaseCost)
        : this(productId, quantity, sellingPrice, purchaseCost)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(saleLineId, nameof(saleLineId));
            SaleLineId = saleLineId;
        }

        private SaleLine(int productId, int quantity, Money sellingPrice, Money purchaseCost)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));

            ArgumentNullException.ThrowIfNull(sellingPrice, nameof(sellingPrice));
            ArgumentNullException.ThrowIfNull(purchaseCost, nameof(purchaseCost));

            if (sellingPrice.Amount <= 0)
                throw new InvalidOperationException("The product selling price cannot be 0 or negative.");

            if (purchaseCost.Amount <= 0)
                throw new InvalidOperationException("The product purchase cost cannot be 0 or negative.");

            ProductId = productId;
            Quantity = quantity;
            SellingPrice = sellingPrice;
            PurchaseCost = purchaseCost;
            DiscountAmount = Money.Zero(sellingPrice.Currency);
        }

        public static SaleLine Create(int productId, int quantity, Money sellingPrice, Money purchaseCost)
        {
            return new SaleLine(productId, quantity, sellingPrice, purchaseCost);
        }

        internal void IncreaseQuantity(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            Quantity += quantity;
        }

        public void ApplyDiscount(Money discountAmount)
        {
            ArgumentNullException.ThrowIfNull(discountAmount, nameof(discountAmount));

            if (discountAmount.Currency != SellingPrice.Currency)
                throw new InvalidOperationException("Currency mismatch.");

            if (discountAmount.Amount > TotalAmount.Amount)
                throw new InvalidOperationException("Discount amount cannot exceed total sale amount.");

            DiscountAmount = discountAmount;
        }
    }
}
