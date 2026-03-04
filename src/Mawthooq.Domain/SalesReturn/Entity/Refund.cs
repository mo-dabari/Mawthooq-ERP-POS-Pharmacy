using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Payment.Enums;
using Mawthooq.Domain.SalesReturn.Enums;
using Mawthooq.Domain.ValueObjects.Money;

namespace Mawthooq.Domain.SalesReturn.Entity
{
    public class Refund
    {
        public int RefundId { get; }
        public Money Amount { get; private set; }
        public RefundStatus Status { get; private set; }
        public PaymentMethod PaymentMethod { get; }
        public Refund(int refundId, Money amount, PaymentMethod paymentMethod)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(refundId, nameof(refundId));

            ArgumentNullException.ThrowIfNull(amount);

            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                throw new InvalidEnumArgumentException(nameof(paymentMethod), (int)paymentMethod, typeof(PaymentMethod));

            RefundId = refundId;
            Amount = amount;
            Status = RefundStatus.Pending;
            PaymentMethod = paymentMethod;
        }

        public void Approve()
        {
            if (Status != RefundStatus.Pending)
                throw new InvalidOperationException("Only pending refunds can be approved.");
            Status = RefundStatus.Approved;
        }

        public void Reject()
        {
            if (Status != RefundStatus.Pending)
                throw new InvalidOperationException("Only pending refunds can be rejected.");
            Status = RefundStatus.Rejected;
        }

        public void MarkAsRefunded()
        {
            if (Status != RefundStatus.Approved)
                throw new InvalidOperationException("Only approved refunds can be marked as refunded.");
            Status = RefundStatus.Refunded;
        }

        internal void IncreaseAmount(Money amount)
        {
            ArgumentNullException.ThrowIfNull(amount, nameof(amount));
            if (amount.Currency != Amount.Currency)
                throw new ArgumentException($"Amount currency {amount.Currency} does not match refund currency {Amount.Currency}.");

            Amount += amount;
        }
    }
}
