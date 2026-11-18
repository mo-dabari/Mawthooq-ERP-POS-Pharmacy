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
    public class NationalIdTests
    {
        private static readonly Dictionary<CountryCode, string> ValidSamples = new()
        {
            [CountryCode.EG] = "29001010101010",
            [CountryCode.SA] = "1234567890",
            [CountryCode.AE] = "784199012345678",
            [CountryCode.QA] = "28412345678",
            [CountryCode.KW] = "289010112345",
            [CountryCode.OM] = "12345678",
            [CountryCode.BH] = "123456789",
            [CountryCode.PS] = "123456789",
            [CountryCode.JO] = "9871234567",
        };

        public static IEnumerable<object[]> Countries()
        {
            return Enum.GetValues<CountryCode>().Select(c => new object[] { c });
        }

        [Fact]
        public void Should_HaveASampleForEveryCountryCode()
        {
            ValidSamples.Keys.Should().BeEquivalentTo(Enum.GetValues<CountryCode>());
        }

        [Theory]
        [MemberData(nameof(Countries))]
        public void Should_AcceptAValidNationalId_ForEveryCountry(CountryCode country)
        {
            var sut = new NationalId(country, ValidSamples[country]);

            sut.Value.Should().Be(ValidSamples[country]);
        }

        [Theory]
        [MemberData(nameof(Countries))]
        public void Should_Throw_When_TheLengthIsTooLong_ForEveryCountry(CountryCode country)
        {
            string valid = ValidSamples[country];
            string tooLongId = valid + "0";

            var act = () => new NationalId(country, tooLongId);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(Countries))]
        public void Should_Throw_When_TheLengthIsTooShort_ForEveryCountry(CountryCode country)
        {
            string valid = ValidSamples[country];
            string tooShortId = valid[..^1];

            var act = () => new NationalId(country, tooShortId);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(Countries))]
        public void Should_Throw_When_TheValueContainsLetters_ForEveryCountry(CountryCode country)
        {
            var act = () => new NationalId(country, "a" + ValidSamples[country][1..]);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Trim_When_TheValueHasSpaces()
        {
            var sut = new NationalId(CountryCode.EG, "  29001010101010  ");

            sut.Value.Should().Be("29001010101010");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_ThrowArgumentException_When_TheValueIsBlank(string? value)
        {
            var act = () => new NationalId(CountryCode.EG, value!);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_TheCountryIsUndefined()
        {
            var act = () => new NationalId((CountryCode)12345, "123");

            act.Should().Throw<InvalidEnumArgumentException>();
        }

        [Fact]
        public void Should_BeEqual_When_CountryAndValueMatch()
        {
            var sut = new NationalId(CountryCode.SA, "1234567890");

            sut.Should().Be(new NationalId(CountryCode.SA, "1234567890"));
        }
    }
}
