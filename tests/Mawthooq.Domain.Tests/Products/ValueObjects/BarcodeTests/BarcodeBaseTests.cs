using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.BarcodeTests
{
    public class BarcodeBaseTests
    {
        private sealed record class TestBarcode : Barcode
        {
            public TestBarcode(string original, BarcodeType type) : base(original, type)
            {
            }
        }

        public class Constructor
        {
            [Theory]
            [InlineData("1234", BarcodeType.Internal)]
            [InlineData("Barcode1234", BarcodeType.QRCode)]
            [InlineData("B", BarcodeType.EAN13)]
            [InlineData("code128", BarcodeType.Code128)]
            public void Should_Create_Barcode_When_Value_Is_Valid(string value, BarcodeType type)
            {
                Barcode barcodeBase = CreateTestBarcode(value, type);

                barcodeBase.Value.Should().Be(value);
                barcodeBase.Type.Should().Be(type);
            }

            [Theory]
            [InlineData("  ")]
            [InlineData("")]
            [InlineData(null)]
            public void Should_Throw_Exception_When_Value_Is_Invalid(string? value)
            {
                Action act = () => CreateTestBarcode(value!, BarcodeType.QRCode);

                act.Should().Throw<ArgumentException>();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            [InlineData(999)]
            public void Should_Throw_Exception_When_BarcodeType_Is_Invalid_Enum(int value)
            {
                Action act = () => CreateTestBarcode("100000", (BarcodeType)value);

                act.Should().Throw<ArgumentException>();
            }
        }

        public class Equality
        {
            [Fact]
            public void Should_Be_Objects_Equal_When_same_Value_And_BarcodeType()
            {
                Barcode bC1 = CreateTestBarcode("1234", BarcodeType.EAN13);
                Barcode bC2 = CreateTestBarcode("1234", BarcodeType.EAN13);

                bC1.Should().Be(bC2);
            }

            [Fact]
            public void Should_Not_Be_Equal_When_Not_Same_Value()
            {
                Barcode bC1 = CreateTestBarcode("AAAA", BarcodeType.EAN13);
                Barcode bC2 = CreateTestBarcode("1234", BarcodeType.EAN13);

                bC1.Should().NotBe(bC2);
            }

            [Fact]
            public void Should_Not_Be_Equal_When_Not_Same_BarcodeType()
            {
                Barcode bC1 = CreateTestBarcode("1234", BarcodeType.Code128);
                Barcode bC2 = CreateTestBarcode("1234", BarcodeType.EAN13);

                bC1.Should().NotBe(bC2);
            }
        }

        public class ToStringMethod
        {
            [Fact]
            public void Should_Return_Value_When_ToString_Called()
            {
                string value = "123456789";

                Barcode barcode = CreateTestBarcode(value, BarcodeType.EAN13);

                barcode.Value.Should().Be(value);
            }
        }

        private static TestBarcode CreateTestBarcode(string value, BarcodeType type) => new TestBarcode(value, type);

    }
}
