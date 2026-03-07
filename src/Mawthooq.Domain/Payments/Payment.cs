using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Payments.Entity;
using Mawthooq.Domain.Payments.Enums;
using Mawthooq.Domain.ValueObjects.Money;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Domain.Payments
{
    public class Payment
    {
        public int PaymentId { get; }
        public int SaleId { get; private set; }
        public DateTime PaidAt { get; }
        public Currency Currency { get; }
        private List<PaymentLine> _paymentLines;
        public IReadOnlyList<PaymentLine> PaymentLines => _paymentLines.AsReadOnly();
        public Money TotalAmount => _paymentLines.Aggregate(Money.Zero(Currency), (sum, line) => sum + line.Amount);

        public Payment(int paymentId, int saleId, Currency currency)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(paymentId, nameof(paymentId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(saleId, nameof(saleId));

            if (!Enum.IsDefined(typeof(Currency), currency))
                throw new InvalidEnumArgumentException(nameof(currency), (int)currency, typeof(Currency));

            PaymentId = paymentId;
            SaleId = saleId;
            Currency = currency;
            PaidAt = DateTime.UtcNow;
            _paymentLines = new();
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
    }
}
