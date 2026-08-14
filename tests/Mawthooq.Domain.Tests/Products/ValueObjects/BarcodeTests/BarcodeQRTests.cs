using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.BarcodeTests
{
    public class BarcodeQRTests
    {
        public class Constructor
        {
            [Theory]
            [InlineData("ABC123")]
            [InlineData("ABC-123")]
            [InlineData("HELLO/WORLD")]
            [InlineData("A+B=C")]
            public void Should_Create_With_Valid_QR(string value)
            {
                BarcodeQR barcodeQR = CreateBarcodeQR(value);

                barcodeQR.Value.Should().Be(value);
                barcodeQR.Type.Should().Be(BarcodeType.QRCode);
            }

            [Theory]
            [InlineData("مرحبا")]
            [InlineData("😊")]
            [InlineData("ABC\t123")]
            [InlineData("ABC\n123")]
            public void Should_Throw_Exception_With_Invalid_QR(string value)
            {
                Action act = () => CreateBarcodeQR(value);

                act.Should().Throw<ArgumentException>();
            }
        }

        public class Equality
        {
            [Fact]
            public void Should_Be_Objects_Equal_When_same_Value_And_BarcodeType()
            {
                BarcodeQR bC1 = CreateBarcodeQR("ABC123");
                BarcodeQR bC2 = CreateBarcodeQR("ABC123");

                bC1.Should().Be(bC2);
            }

            [Fact]
            public void Should_Not_Be_Equal_When_Not_Same_Value()
            {
                BarcodeQR bC1 = CreateBarcodeQR("ABC123");
                BarcodeQR bC2 = CreateBarcodeQR("WORLD");

                bC1.Should().NotBe(bC2);
            }
        }

        public class ToStringMethod
        {
            [Fact]
            public void Should_Return_Value_When_ToString_Called()
            {
                string value = "HELLO/WORLD";

                BarcodeQR barcode = CreateBarcodeQR(value);

                barcode.Value.Should().Be(value);
            }
        }

        private static BarcodeQR CreateBarcodeQR(
            string value) => BarcodeQR.New(value);
    }
}

