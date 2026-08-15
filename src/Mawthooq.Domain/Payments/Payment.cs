using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Payments.Entity;
using Mawthooq.Domain.Payments.Enums;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Payments
{
    public class Payment
    {
        public int PaymentId { get; private set; }
        public int SaleId { get; }
        public DateTime PaidAt { get; }
        public Currency Currency { get; }
        private List<PaymentLine> _paymentLines = new();
        public IReadOnlyList<PaymentLine> PaymentLines => _paymentLines.AsReadOnly();
        public Money TotalAmount => _paymentLines.Aggregate(Money.Zero(Currency), (sum, line) => sum + line.Amount);

        public Payment(int paymentId, int saleId, Currency currency) : this(saleId, currency)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(paymentId, nameof(paymentId));
            PaymentId = paymentId;
        }

        private Payment(int saleId, Currency currency)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(saleId, nameof(saleId));

            if (!Enum.IsDefined(typeof(Currency), currency))
                throw new InvalidEnumArgumentException(nameof(currency), (int)currency, typeof(Currency));

            SaleId = saleId;
            Currency = currency;
            PaidAt = DateTime.UtcNow;
        }

        public static Payment Create(int saleId, Currency currency)
        {
            return new Payment(saleId, currency);
        }

        public void AddPaymentLine(Money amount, PaymentMethod paymentMethod)
        {
            if (amount.Currency != Currency)
                throw new ArgumentException($"Payment line currency {amount.Currency} does not match payment currency {Currency}.");

            PaymentLine? existing = _paymentLines.FirstOrDefault(line => line.PaymentMethod == paymentMethod);

            if (existing is not null)
            {
                existing.IncreaseAmount(amount);
                return;
            }
            _paymentLines.Add(new PaymentLine(amount, paymentMethod));
        }
        public void SetPaymentId(int paymentId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(paymentId, nameof(paymentId));
            if (PaymentId != 0)
                throw new InvalidOperationException("Payment ID has already been set.");

            PaymentId = paymentId;
        }
    }
}
