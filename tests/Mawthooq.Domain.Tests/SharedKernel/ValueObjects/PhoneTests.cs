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
    public class PhoneTests
    {
        private static readonly Dictionary<CountryCode, (string Local, string International)> Samples = new()
        {
            [CountryCode.EG] = ("01012345678", "+201012345678"),
            [CountryCode.SA] = ("0512345678", "+966512345678"),
            [CountryCode.AE] = ("0512345678", "+971512345678"),
            [CountryCode.JO] = ("0712345678", "+962712345678"),
            [CountryCode.PS] = ("0591234567", "+970591234567"),
            [CountryCode.QA] = ("33123456", "+97433123456"),
            [CountryCode.KW] = ("51234567", "+96551234567"),
            [CountryCode.OM] = ("91234567", "+96891234567"),
            [CountryCode.BH] = ("31234567", "+97331234567"),
        };

        public static IEnumerable<object[]> Countries()
        {
            return Enum.GetValues<CountryCode>().Select(c => new object[] { c });
        }

        [Fact]
        public void Should_HaveASampleForEveryCountryCode()
        {
            Samples.Keys.Should().BeEquivalentTo(Enum.GetValues<CountryCode>());
        }

        [Fact]
        public void Should_UseTheRealCallingCodeForEgypt()
        {
            ((int)CountryCode.EG).Should().Be(20);

        }

        [Theory]
        [MemberData(nameof(Countries))]
        public void Should_AcceptAValidLocalNumber_ForEveryCountry(CountryCode country)
        {
            var sut = new Phone(country, Samples[country].Local);

            sut.Value.Should().Be(Samples[country].Local);
        }

        [Theory]
        [MemberData(nameof(Countries))]
        public void Should_FormatAsInternational_WithoutTheLocalLeadingZero(CountryCode country)
        {
            var sut = new Phone(country, Samples[country].Local);

            sut.ToString().Should().Be(Samples[country].International);
        }

        [Theory]
        [InlineData(CountryCode.EG, "01312345678")]
        [InlineData(CountryCode.EG, "0101234567")]
        [InlineData(CountryCode.EG, "0101234567a")]
        [InlineData(CountryCode.SA, "0412345678")]
        [InlineData(CountryCode.QA, "23123456")]
        public void Should_ThrowArgumentException_When_TheNumberDoesNotMatchTheCountryFormat(CountryCode country, string number)
        {
            var act = () => new Phone(country, number);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_ThrowArgumentException_When_TheNumberIsBlank(string? number)
        {
            var act = () => new Phone(CountryCode.EG, number!);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_DefaultToThePrimaryType()
        {
            var sut = new Phone(CountryCode.EG, "01012345678");

            sut.PhoneType.Should().Be(PhoneType.Primary);
        }

        [Fact]
        public void Should_Throw_When_TheCountryIsUndefined()
        {
            var act = () => new Phone((CountryCode)12345, "01012345678");

            act.Should().Throw<InvalidEnumArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_TheTypeIsUndefined()
        {
            var act = () => new Phone(CountryCode.EG, "01012345678", (PhoneType)99);

            act.Should().Throw<InvalidEnumArgumentException>();
        }

        [Fact]
        public void Should_BeEqual_When_CountryNumberAndTypeMatch()
        {
            var sut = new Phone(CountryCode.EG, "01012345678");

            sut.Should().Be(new Phone(CountryCode.EG, "01012345678"));
        }
    }
}
