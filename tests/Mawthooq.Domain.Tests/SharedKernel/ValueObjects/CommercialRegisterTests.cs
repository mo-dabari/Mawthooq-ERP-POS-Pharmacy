using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace Mawthooq.Domain.Tests.SharedKernel.ValueObjects
{
    public class CommercialRegisterTests
    {
        [Theory]
        [InlineData("123", "123")]
        [InlineData("12345678", "12345678")]
        [InlineData("  12345  ", "12345")]
        public void Should_Trim_When_TheValueIsValid(string input, string expected)
        {
            var sut = CreateCommercialRegister(input);

            sut.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_ThrowArgumentException_When_TheValueIsNullOrWhiteSpace(string? input)
        {
            var act = () => CreateCommercialRegister(input!);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("12a45")]
        [InlineData("12 345")]
        public void Should_ThrowArgumentException_When_TheValueIsNotDigit(string? input)
        {
            var act = () => CreateCommercialRegister(input!);

            act.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData("12")]
        [InlineData("123456789")]
        public void Should_ThrowArgumentException_When_ValueIsOutOfRange(string? input)
        {
            var act = () => CreateCommercialRegister(input!);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Should_BeEqual_When_TheTrimmedValuesMatch()
        {
            var sut = CreateCommercialRegister(" 12345 ");

            sut.Should().Be(new CommercialRegister("12345"));
        }

        private CommercialRegister CreateCommercialRegister(string value)
        {
            return new CommercialRegister(value);
        }
    }
}
