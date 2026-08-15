using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Payments.Enums;
using Mawthooq.Domain.Returns.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Returns.Entity
{
    public class Refund
    {
        public int RefundId { get; }
        public Money Amount { get; private set; }
        public RefundStatus Status { get; private set; }
        public PaymentMethod PaymentMethod { get; }
        public Refund(int refundId, Money amount, PaymentMethod paymentMethod)
        : this(amount, paymentMethod)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(refundId, nameof(refundId));
            RefundId = refundId;
        }

        private Refund(Money amount, PaymentMethod paymentMethod)
        {
            ArgumentNullException.ThrowIfNull(amount,nameof(amount));

            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                throw new InvalidEnumArgumentException(nameof(paymentMethod), (int)paymentMethod, typeof(PaymentMethod));

            Amount = amount;
            Status = RefundStatus.Pending;
            PaymentMethod = paymentMethod;
        }

        public static Refund Create(Money amount, PaymentMethod paymentMethod)
        {
            return new Refund(amount, paymentMethod);
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
