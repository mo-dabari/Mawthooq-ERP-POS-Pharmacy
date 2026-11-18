using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.BarcodeTests
{
    public class UnitBarcodeFactoryTests
    {
        [Fact]
        public void Should_CreateAnEan13Barcode()
        {
            var sut = UnitBarcodeFactory.Create("5000000000017", BarcodeType.EAN13);

            sut.Should().BeOfType<BarcodeEAN13Linear>()
            .Which.Type.Should().Be(BarcodeType.EAN13);
        }

        [Fact]
        public void Should_CreateAQrBarcode()
        {
            var sut = UnitBarcodeFactory.Create("PRODUCT-QR-1", BarcodeType.QRCode);

            sut.Should().BeOfType<BarcodeQR>().Which.Value.Should().Be("PRODUCT-QR-1");
        }

        [Fact]
        public void Should_CreateAnInternalBarcode()
        {
            var sut = UnitBarcodeFactory.Create("1000", BarcodeType.Internal);

            sut.Should().BeOfType<InternalLinearBarcode>().Which.Type.Should().Be(BarcodeType.Internal);
        }

        [Theory]
        [InlineData(BarcodeType.UPC)]
        [InlineData(BarcodeType.Code128)]
        [InlineData(BarcodeType.DataMatrix)]
        [InlineData((BarcodeType)99)]
        public void Should_Throw_When_TheTypeIsNotSupported(BarcodeType type)
        {
            var act = () => UnitBarcodeFactory.Create("12345678", type);

            act.Should().Throw<NotSupportedException>();
        }

        [Theory]
        [InlineData("123", BarcodeType.EAN13)]
        [InlineData("abc", BarcodeType.Internal)]
        public void Should_Throw_When_TheValueDoesNotFitTheType(string value, BarcodeType type)
        {
            var act = () => UnitBarcodeFactory.Create(value, type);

            act.Should().Throw<ArgumentException>();
        }
    }
}
