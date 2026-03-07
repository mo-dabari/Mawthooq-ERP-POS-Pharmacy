using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Sales.ValueObjects.Discount.Enums;
using Mawthooq.Domain.ValueObjects.Money;

namespace Mawthooq.Domain.Sales.ValueObjects.Discount
{
    public class Discount
    {
        public string Name { get; private set; }

        public int? ProductId { get; }
        public DiscountType DiscountType { get; }
        public DiscountTarget DiscountTarget { get; }
        public decimal Value { get; private set; }
        public bool IsActive { get; private set; }

        public Discount(string name, int? productId, DiscountType discountType, DiscountTarget discountTarget, decimal value, bool isActive)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            if (!Enum.IsDefined(typeof(DiscountType), discountType))
                throw new InvalidEnumArgumentException(nameof(discountType), (int)discountType, typeof(DiscountType));

            if (!Enum.IsDefined(typeof(DiscountTarget), discountTarget))
                throw new InvalidEnumArgumentException(nameof(discountTarget), (int)discountTarget, typeof(DiscountTarget));

            if (discountTarget == DiscountTarget.Product && productId is null)
                throw new ArgumentException("ProductId is required for product discounts.");

            if (discountType == DiscountType.Percentage && value > 100)
                throw new ArgumentOutOfRangeException(nameof(value), "Percentage cannot exceed 100.");

            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));

            Name = name;
            ProductId = productId;
            DiscountType = discountType;
            DiscountTarget = discountTarget;
            Value = value;
            IsActive = isActive;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public Money CalculateDiscountAmount(Money originalPrice)
        {
            if (!IsActive) return Money.Zero(originalPrice.Currency);

            return DiscountType switch
            {
                DiscountType.Percentage => new Money(originalPrice.Amount * (Value / 100), originalPrice.Currency) ,
                DiscountType.FixedAmount => new Money(Math.Min(Value, originalPrice.Amount), originalPrice.Currency) ,
                _ => throw new InvalidOperationException("Invalid discount type.")
            };
        }

    }
}
