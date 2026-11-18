using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace Mawthooq.Domain.Tests.SharedKernel.ValueObjects
{
    public class TaxNumberTests
    {
        [Theory]
        [InlineData("123456789")]
        [InlineData("123-456-789")]
        [InlineData(" 123 456 789 ")]
        [InlineData("123-456 789")]
        public void Should_StoreTheDigitsOnly_When_TheTaxNumberIsValid(string input)
        {
            var sut = CreateTaxNumber(input);
            sut.Value.Should().Be("123456789");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]

        [InlineData("12345678")]
        [InlineData("1234567890")]
        [InlineData("12345678a")]
        [InlineData("abcdefghi")]
        public void Should_ThrowArgumentException_When_TheTaxNumberIsNullOrWhiteSpace(string? input)
        {
            var act = () => CreateTaxNumber(input!);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("12345678a")]
        [InlineData("abcdefghi")]
        public void Should_ThrowArgumentException_When_TheTaxNumberIsNotDigit(string? input)
        {
            var act = () => CreateTaxNumber(input!);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("1234567890")]
        public void Should_ThrowArgumentException_When_ValueIsOutOfRange(string? input)
        {
            var act = () => CreateTaxNumber(input!);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_FormatAsThreeGroupsOfThreeDigits() =>
            new TaxNumber("123456789").ToFormattedString().Should().Be("123-456-789");

        [Fact]
        public void Should_BeEqual_When_TheDigitsMatch() =>
            new TaxNumber("123-456-789").Should().Be(new TaxNumber("123456789"));

        private TaxNumber CreateTaxNumber(string value)
        {
            return new TaxNumber(value);
        }
    }
}
