using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Mawthooq.Domain.Discounts.Enums;
using Mawthooq.Domain.ValueObjects.Money;

namespace Mawthooq.Domain.Discounts
{
    public class Discount
    {
        public int DiscountId { get; }
        public string Name { get; }
        public int? ProductId { get; }
        public DiscountType DiscountType { get; }
        public DiscountTarget DiscountTarget { get; }
        public Money? MinimumAmount { get; }
        public decimal Value { get; }
        public bool IsActive { get; private set; }

        public Discount(int discountId, string name, int? productId, DiscountType discountType, DiscountTarget discountTarget, decimal value, bool isActive)
        : this(name, productId, discountType, discountTarget, value, isActive)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(discountId, nameof(discountId));
            DiscountId = discountId;
        }


        private Discount(string name, int? productId, DiscountType discountType, DiscountTarget discountTarget, decimal value, bool isActive, Money? minimumAmount = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            if (!Enum.IsDefined(typeof(DiscountType), discountType))
                throw new InvalidEnumArgumentException(nameof(discountType), (int)discountType, typeof(DiscountType));

            if (!Enum.IsDefined(typeof(DiscountTarget), discountTarget))
                throw new InvalidEnumArgumentException(nameof(discountTarget), (int)discountTarget, typeof(DiscountTarget));

            if (discountTarget == DiscountTarget.Product && productId is null)
                throw new ArgumentException("ProductId is required for product discounts.", nameof(productId));

            if (discountType == DiscountType.Percentage && value > 100)
                throw new ArgumentOutOfRangeException(nameof(value), "Percentage cannot exceed 100.");

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(value));

            Name = name;
            ProductId = productId;
            DiscountType = discountType;
            DiscountTarget = discountTarget;
            Value = value;
            IsActive = isActive;
            MinimumAmount = minimumAmount;
        }

        public static Discount Create(string name, int? productId, DiscountType discountType, DiscountTarget discountTarget, decimal value, bool isActive, Money? minimumAmount = null)
        {
            return new Discount(name, productId, discountType, discountTarget, value, isActive, minimumAmount);
        }

        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
        }
        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
        }

        public Money CalculateDiscountAmount(Money originalPrice)
        {
            if (!IsActive) return Money.Zero(originalPrice.Currency);

            return DiscountType switch
            {
                DiscountType.Percentage => new Money(originalPrice.Amount * (Value / 100), originalPrice.Currency),
                DiscountType.FixedAmount => new Money(Math.Min(Value, originalPrice.Amount), originalPrice.Currency),
                _ => throw new InvalidOperationException("Invalid discount type.")
            };
        }
    }
}
