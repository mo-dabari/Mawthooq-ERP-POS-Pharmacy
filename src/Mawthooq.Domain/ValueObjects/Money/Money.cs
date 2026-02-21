using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Domain.ValueObjects.Money
{
    public class Money
    {
        public decimal Amount { get; init; }
        public Currency Currency { get; init; }
        public Money(decimal amount, Currency currency)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount, nameof(amount));

            if (!Enum.IsDefined(typeof(Currency), currency))
                throw new InvalidEnumArgumentException(nameof(currency), (int)currency, typeof(Currency));

            Amount = amount;
            Currency = currency;
        }

        public static Money operator +(Money firstMony, Money scondMony)
        {
            if (firstMony.Currency != scondMony.Currency)
                throw new InvalidOperationException("Different currencies cannot be collected");

            return new Money(firstMony.Amount + scondMony.Amount, scondMony.Currency);
        }

        public bool Equals(Money? other)
        {
            if (other is null)
                return false;

            return Amount == other.Amount &&
                   Currency == other.Currency;
        }

        public override bool Equals(object? obj)
            => Equals(obj as Money);

        public static bool operator ==(Money? left, Money? right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null || right is null)
                return false;

            return Equals(left, right);
        }
        public static bool operator !=(Money? left, Money? right) => !(left == right);
        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}
