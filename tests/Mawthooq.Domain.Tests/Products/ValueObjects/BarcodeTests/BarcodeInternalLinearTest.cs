using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.BarcodeTests
{
    public class BarcodeInternalLinearTest
    {
        public class Constructor
        {
            [Fact]
            public void Should_Create_With_Valid_InternalLinear()
            {
                string value = "123456789";
                InternalLinearBarcode InternalLinearBarcode = CreateInternalLinearBarcode(value);

                InternalLinearBarcode.Value.Should().Be(value);
                InternalLinearBarcode.Type.Should().Be(BarcodeType.Internal);
            }

            [Fact]
            public void Should_Throw_Exception_With_Invalid_InternalLinear()
            {
                Action act = () => CreateInternalLinearBarcode("1234567891");

                act.Should().Throw<ArgumentException>();
            }
        }

        public class Equality
        {
            [Fact]
            public void Should_Be_Objects_Equal_When_same_Value_And_BarcodeType()
            {
                InternalLinearBarcode bC1 = CreateInternalLinearBarcode("123456789");
                InternalLinearBarcode bC2 = CreateInternalLinearBarcode("123456789");

                bC1.Should().Be(bC2);
            }

            [Fact]
            public void Should_Not_Be_Equal_When_Not_Same_Value()
            {
                InternalLinearBarcode bC1 = CreateInternalLinearBarcode("123456789");
                InternalLinearBarcode bC2 = CreateInternalLinearBarcode("123456788");

                bC1.Should().NotBe(bC2);
            }
        }

        public class ToStringMethod
        {
            [Fact]
            public void Should_Return_Value_When_ToString_Called()
            {
                string value = "123456789";

                InternalLinearBarcode barcode = CreateInternalLinearBarcode(value);

                barcode.Value.Should().Be(value);
            }
        }

        private static InternalLinearBarcode CreateInternalLinearBarcode(
            string value) => InternalLinearBarcode.New(value);
    }
}
