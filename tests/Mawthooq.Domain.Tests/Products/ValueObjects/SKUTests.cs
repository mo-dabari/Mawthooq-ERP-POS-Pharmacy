using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.ValueObjects;
using Xunit;

namespace Mawthooq.Domain.Tests.Products.ValueObjects
{
    public class SKUTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_CreateSKU_When_StateIsValid()
            {
                string value = "VALID-SKU-123";

                SKU sku = CreateSKU(value);

                sku.Value.Should().Be(value.ToUpperInvariant());
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_SKUIsNull()
            {
                Action act = () => CreateSKU(null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [InlineData("")]
            [InlineData("   ")]
            [InlineData("SHORT")]
            [InlineData("THIS-SKU-IS-WAY-TOO-LONG-AND-SHOULD-FAIL-BECAUSE-IT-EXCEEDS-FIFTY-CHARACTERS")]
            [InlineData("INVALID SKU!")]
            public void Constructor_Should_ThrowArgumentException_When_SKUIsInvalid(string value)
            {
                Action act = () => CreateSKU(value);

                act.Should().Throw<ArgumentException>();
            }

        }

        public class Equality
        {
            [Fact]
            public void Should_Be_Equal_When_Values_Are_Same()
            {
                string value = "VALID-SKU-123";

                SKU sku1 = CreateSKU(value);
                SKU sku2 = CreateSKU(value);

                sku1.Should().Be(sku2);
            }

            [Fact]
            public void Should_Not_Be_Equal_When_Values_Are_Different()
            {
                SKU sku1 = CreateSKU("VALID-SKU-123");
                SKU sku2 = CreateSKU("DIFFERENT-SKU-456");

                sku1.Should().NotBe(sku2);
            }
        }

        public class ToStringMethod
        {
            [Fact]
            public void Should_Return_Value_When_ToString_Called()
            {
                string value = "VALID-SKU-123";
                SKU sku = CreateSKU(value);

                string result = sku.ToString();

                result.Should().Be(value.ToUpperInvariant());
            }
        }
        private static SKU CreateSKU(string value) => SKU.Create(value);

    }
}
