using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Domain.SharedKernel.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; init; }
        public Currency Currency { get; init; }
        public Money(decimal amount, Currency currency)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));

            if (!Enum.IsDefined(typeof(Currency), currency))
                throw new InvalidEnumArgumentException(nameof(currency), (int)currency, typeof(Currency));

            Amount = amount;
            Currency = currency;
        }
        public static Money Zero(Currency currency)
        {
            return new Money(0m, currency);
        }

        public Money Add(Money other)
        {
            ArgumentNullException.ThrowIfNull(other, nameof(other));

            if (Currency != other.Currency)
                throw new InvalidOperationException("Different currencies cannot be collected");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            ArgumentNullException.ThrowIfNull(other, nameof(other));

            if (Currency != other.Currency)
                throw new InvalidOperationException("Different currencies cannot be collected");

                if (Amount < other.Amount)
                    throw new InvalidOperationException("Cannot subtract a larger amount from a smaller one");

            return new Money(Amount - other.Amount, Currency);
        }

        public Money Multiply(int factor)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(factor, nameof(factor));
            return new Money(Amount * factor, Currency);
        }


        public static Money operator +(Money left, Money right)
        {
            ArgumentNullException.ThrowIfNull(left, nameof(left));
            ArgumentNullException.ThrowIfNull(right, nameof(right));

            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Different currencies cannot be collected");

            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            ArgumentNullException.ThrowIfNull(left, nameof(left));
            ArgumentNullException.ThrowIfNull(right, nameof(right));

            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Different currencies cannot be collected");

            return new Money(left.Amount - right.Amount, left.Currency);
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
