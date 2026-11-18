using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace Mawthooq.Domain.Tests.SharedKernel.ValueObjects
{
    public class MoneyTests
    {
        private static Money Egp(decimal amount)
        {
            return new(amount, Currency.EGP);
        }

        private static Money Usd(decimal amount)
        {
            return new(amount, Currency.USD);
        }


        [Fact]
        public void Should_KeepAmountAndCurrency_When_Created()
        {
            Money money = Egp(10.5m);

            money.Amount.Should().Be(10.5m);
            money.Currency.Should().Be(Currency.EGP);
        }

        [Fact]
        public void Should_AllowZero()
        {
            var sut = Egp(0m);

            sut.Amount.Should().Be(0m);
        }


        [Fact]
        public void Should_Throw_When_TheAmountIsNegative()
        {
            var act = () => Egp(-0.01m);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Should_Throw_When_TheCurrencyIsUndefined()
        {
            var act = () => new Money(1m, (Currency)999);

            act.Should().Throw<InvalidEnumArgumentException>();
        }

        [Fact]
        public void Zero_Should_BeZeroInTheGivenCurrency()
        {
            var sut = Money.Zero(Currency.SAR);
            var expect = new Money(0m, Currency.SAR);

            sut.Should().Be(expect);
        }

        [Fact]
        public void Add_Should_SumTheAmounts()
        {
            var sut = Egp(10m);
            sut = sut.Add(Egp(2.5m));

            var expect = Egp(12.5m);

            sut.Should().Be(expect);
        }

        [Fact]
        public void Add_Should_Throw_When_TheCurrenciesDiffer()
        {
            var sut = Egp(1m);
            var act = () => sut.Add(Usd(1m));

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Add_Should_Throw_When_TheOtherIsNull()
        {
            var sut = Egp(1m);
            var act = () => sut.Add(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Subtract_Should_ReturnTheDifference()
        {
            var sut = Egp(10m);
            sut = sut.Subtract(Egp(4m));

            sut.Should().Be(Egp(6m));
        }

        [Fact]
        public void Subtract_Should_Throw_When_TheResultWouldBeNegative()
        {
            var sut = Egp(1m);

            var act = () => sut.Subtract(Egp(2m));

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Subtract_Should_Throw_When_TheCurrenciesDiffer()
        {
            var sut = Egp(5m);
            var act = () => sut.Subtract(Usd(1m));

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Multiply_Should_ScaleTheAmount()
        {
            var sut = Egp(2.5m);
            sut = sut.Multiply(4);

            sut.Should().Be(Egp(10m));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Multiply_Should_Throw_When_TheFactorIsNotPositive(int factor)
        {
            var sut = Egp(1m);

            var act = () => sut.Multiply(factor);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void PlusOperator_Should_BehaveLikeAdd()
        {
            var sut = (Egp(1m) + Egp(2m));

            sut.Should().Be(Egp(3m));
        }

        [Fact]
        public void MinusOperator_Should_BehaveLikeSubtract()
        {
            var sut = (Egp(5m) - Egp(2m));

            sut.Should().Be(Egp(3m));
        }

        [Fact]
        public void Should_BeEqual_When_AmountAndCurrencyMatch_RegardlessOfScale()
        {
            Money left = Egp(10m),
            right = Egp(10.00m);

            left.Should().Be(right);
            (left == right).Should().BeTrue();
            (left != right).Should().BeFalse();
            left.GetHashCode().Should().Be(right.GetHashCode());
        }

        [Fact]
        public void Should_NotBeEqual_When_TheAmountOrTheCurrencyDiffers()
        {
            var sut = Egp(10m);

            sut.Should().NotBe(Egp(11m));
            sut.Should().NotBe(Usd(10m));

            (sut == Usd(10m)).Should().BeFalse();
        }

        [Fact]
        public void Should_HandleNullInTheEqualityOperators()
        {
            Money? none = null;

            (none == null).Should().BeTrue();
            (Egp(1m) == null).Should().BeFalse();
            (none != Egp(1m)).Should().BeTrue();
        }
    }
}
