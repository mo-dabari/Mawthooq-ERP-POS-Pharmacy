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

        public Money TotalAmount => SellingPrice.Multiply(Quantity);
        public Money TotalPurchaseCost => PurchaseCost.Multiply(Quantity);
        public Money GrossProfit => TotalAmount - TotalPurchaseCost;

        internal SaleLine(int saleLineId, int productId, int quantity, Money sellingPrice, Money purchaseCost)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sellingPrice.Amount, nameof(quantity));

            ArgumentNullException.ThrowIfNull(sellingPrice, nameof(sellingPrice));
            ArgumentNullException.ThrowIfNull(purchaseCost, nameof(purchaseCost));

            if (sellingPrice.Amount <= 0)
                throw new InvalidOperationException("The product selling price cannot be 0 or negative.");

            if (purchaseCost.Amount <= 0)
                throw new InvalidOperationException("The product purchase cost cannot be 0 or negative.");

            SaleLineId = saleLineId;
            ProductId = productId;
            Quantity = quantity;
            SellingPrice = sellingPrice;
            PurchaseCost = purchaseCost;
        }

        internal void IncreaseQuantity(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            Quantity += quantity;
        }
    }
}
