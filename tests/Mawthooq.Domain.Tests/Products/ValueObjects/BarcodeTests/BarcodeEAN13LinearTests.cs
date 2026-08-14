using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.BarcodeTests
{
    public class BarcodeEAN13LinearTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_Create_With_Valid_EAN13()
            {
                Barcode EAN13Linear = CreateBarcodeEAN13Linear("1234567891234");

                EAN13Linear.Value.Should().Be("1234567891234");
                EAN13Linear.Type.Should().Be(BarcodeType.EAN13);
            }

            [Theory]
            [InlineData("123456789")]
            [InlineData("12345678912345")]
            [InlineData("ABCD")]
            [InlineData("ABCD123456789")]
            public void Should_Throw_Exception_With_Invalid_EAN13(string value)
            {
                Action act = () => CreateBarcodeEAN13Linear(value);

                act.Should().Throw<ArgumentException>();
            }
        }

        public class Equality
        {
            [Fact]
            public void Should_Be_Objects_Equal_When_same_Value_And_BarcodeType()
            {
                Barcode bC1 = CreateBarcodeEAN13Linear("1234567891234");
                Barcode bC2 = CreateBarcodeEAN13Linear("1234567891234");

                bC1.Should().Be(bC2);
            }

            [Fact]
            public void Should_Not_Be_Equal_When_Not_Same_Value()
            {
                Barcode bC1 = CreateBarcodeEAN13Linear("1234567891235");
                Barcode bC2 = CreateBarcodeEAN13Linear("1234567891234");

                bC1.Should().NotBe(bC2);
            }
        }

        public class ToStringMethod
        {
            [Fact]
            public void Should_Return_Value_When_ToString_Called()
            {
                string value = "1234567891234";

                Barcode barcode = CreateBarcodeEAN13Linear(value);

                barcode.Value.Should().Be(value);
            }
        }

        private static BarcodeEAN13Linear CreateBarcodeEAN13Linear(
            string value) => BarcodeEAN13Linear.New(value);
    }
}
