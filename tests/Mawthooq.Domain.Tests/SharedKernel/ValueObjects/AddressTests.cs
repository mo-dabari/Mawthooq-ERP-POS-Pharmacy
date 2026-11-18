using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace Mawthooq.Domain.Tests.SharedKernel.ValueObjects
{
    public class AddressTests
    {
        private static Address CreateAddressValid() => new(12, "Main", "New Nozha", "Cairo", "Egypt");

        [Fact]
        public void Should_KeepAllTheParts_When_Created()
        {
            Address address = CreateAddressValid();

            address.BuildingNumber.Should().Be(12);
            address.Street.Should().Be("Main");
            address.Neighborhood.Should().Be("New Nozha");
            address.Governorate.Should().Be("Cairo");
            address.Country.Should().Be("Egypt");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Should_Throw_When_TheBuildingNumberIsNotPositive(int number)
        {
            Action act = () => new Address(number, "s", "c", "g", "x");

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(null, "c", "g", "x")]
        [InlineData("", "c", "g", "x")]
        [InlineData("s", "  ", "g", "x")]
        [InlineData("s", "c", null, "x")]
        [InlineData("s", "c", "g", "")]
        public void Should_ThrowArgumentException_When_ATextPartIsBlank(string? street, string? city, string? governorate, string? country)
        {
            var act = () => new Address(1, street!, city!, governorate!, country!);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_JoinThePartsWithDashes_When_ConvertedToText()
        {
            var sut = CreateAddressValid();

            sut.ToString().Should().Be("12-Main St-New Nozha-Cairo-Egypt");
        }

        [Fact]
        public void Should_BeEqual_When_AllThePartsMatch()
        {
            var sut = CreateAddressValid();

            sut.Equals(CreateAddressValid()).Should().BeTrue();
            sut.GetHashCode().Should().Be(CreateAddressValid().GetHashCode());
        }

        [Fact]
        public void Should_NotBeEqual_When_APartDiffers()
        {
            var sut = CreateAddressValid();
            sut.Equals(new Address(13, "Main", "New Nozha", "Cairo", "Egypt")).Should().BeFalse();
        }

        [Fact]
        public void Should_ReturnFalse_When_ComparedWithNull()
        {
            var sut = CreateAddressValid();
            Address? none = null;

            sut.Equals(none!).Should().BeFalse();
            sut.Equals((object?)null).Should().BeFalse();
        }
    }
}
