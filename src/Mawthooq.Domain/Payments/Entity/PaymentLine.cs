using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Payments.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Payments.Entity
{
    public class PaymentLine
    {
        public Money Amount { get; private set; }
        public PaymentMethod PaymentMethod { get; }


        public PaymentLine(Money amount, PaymentMethod paymentMethod)
        {
            ArgumentNullException.ThrowIfNull(amount);

            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                throw new InvalidEnumArgumentException(nameof(paymentMethod), (int)paymentMethod, typeof(PaymentMethod));

            Amount = amount;
            PaymentMethod = paymentMethod;
        }

        public void IncreaseAmount(Money amount)
        {
            ArgumentNullException.ThrowIfNull(amount);
            if (amount.Currency != Amount.Currency)
                throw new ArgumentException($"Amount currency {amount.Currency} does not match payment line currency {Amount.Currency}.");

            Amount += amount;
        }

        public bool Equals(PaymentLine? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return PaymentMethod == other.PaymentMethod;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PaymentMethod);
        }
    }
}
