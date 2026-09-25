using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.SharedKernel.Exceptions;

namespace Mawthooq.Domain.Tests.Products.ValueObjects
{
    public class ProductUnitDetailsTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_CreateProductUnitDetails_When_ProductUnitDetailsIsValid()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                var sut = CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, internalLinearBarcode);

                sut.Barcodes.First().Value.Should().Be(internalLinearBarcode.Value);
                sut.Unit.Should().Be(UnitOfMeasure.Disc);
                sut.ConversionRate.Should().Be(1);
                sut.Rank.Should().Be(UnitRank.Tertiary);

                sut.Barcodes.Should().HaveCount(1);
            }

            [Fact]
            public void Should_ArgumentNullException_When_InternalLinearBarcodeIsNull()
            {
                Action sut = () => CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, null!);

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_InvalidEnumArgumentException_When_UnitOfMeasureIsInvalid()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                Action sut = () => CreateProductUnitDetails((UnitOfMeasure)999, 1, UnitRank.Tertiary, internalLinearBarcode);

                sut.Should().Throw<InvalidEnumArgumentException>();
            }

            [Fact]
            public void Should_InvalidEnumArgumentException_When_UnitRankIsInvalid()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                Action sut = () => CreateProductUnitDetails(UnitOfMeasure.Disc, 1, (UnitRank)999, internalLinearBarcode);

                sut.Should().Throw<InvalidEnumArgumentException>();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_ArgumentOutOfRangeException_When_ConversionRateIsInvalid(int conversionRate)
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                Action sut = () => CreateProductUnitDetails(UnitOfMeasure.Disc, conversionRate, UnitRank.Tertiary, internalLinearBarcode);

                sut.Should().Throw<ArgumentOutOfRangeException>();
            }
        }

        public class CompanyBarcodeBehavior
        {
            [Theory]
            [MemberData(nameof(GetBarcodeTestData))]
            public void Should_AddingBarcodeToUnit_When_BarcodeIsValid(Barcode barcode)
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                var sut = CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, internalLinearBarcode);

                sut = sut.AddCompanyBarcode(barcode);

                Barcode lastBarcode = sut.Barcodes.Last();

                lastBarcode.Value.Should().Be(barcode.Value);
                lastBarcode.Type.Should().Be(barcode.Type);
            }

            [Fact]
            public void Should_ArgumentNullException_When_AddedingBarcodeIsNull()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                var sut = CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, internalLinearBarcode);

                Action act = () => sut.AddCompanyBarcode(null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowDomainRuleViolationException_When_AddedingBarcodeTypeInternal()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                var sut = CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, internalLinearBarcode);

                Action act = () => sut.AddCompanyBarcode(CreateInternalLinearBarcode());

                act.Should().Throw<DomainRuleViolationException>();
            }

            [Fact]
            public void Should_ThrowDomainRuleViolationException_When_AddedingBarcodeAlreadyExists()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                var sut = CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, internalLinearBarcode);

                Barcode barcode = BarcodeEAN13Linear.New("1234567891234");

                sut = sut.AddCompanyBarcode(barcode);

                Action act = () => sut.AddCompanyBarcode(barcode);

                act.Should().Throw<DomainRuleViolationException>();
            }

            [Fact]
            public void Should_ArgumentNullException_When_RemoveingBarcodeIsNull()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                var sut = CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, internalLinearBarcode);

                Action act = () => sut.RemoveCompanyBarcode(null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowDomainRuleViolationException_When_RemoveingBarcodeIsTypeInternal()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                var sut = CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, internalLinearBarcode);

                Action act = () => sut.RemoveCompanyBarcode(internalLinearBarcode);

                act.Should().Throw<DomainRuleViolationException>();
            }

            [Fact]
            public void Should_ThrowDomainRuleViolationException_When_RemoveingBarcodeIsNotFound()
            {
                var internalLinearBarcode = CreateInternalLinearBarcode();

                var sut = CreateProductUnitDetails(UnitOfMeasure.Disc, 1, UnitRank.Tertiary, internalLinearBarcode);

                Barcode notFoundBarcode = BarcodeEAN13Linear.New("1234567891234");

                Action act = () => sut.RemoveCompanyBarcode(notFoundBarcode);

                act.Should().Throw<DomainRuleViolationException>();
            }

            public static IEnumerable<object[]> GetBarcodeTestData()
            {
                yield return new object[] { BarcodeEAN13Linear.New("1234567891234") };
                yield return new object[] { BarcodeQR.New("ABC-123") };
            }
        }

        private static ProductUnitDetails CreateProductUnitDetails(UnitOfMeasure unit, int conversionRate, UnitRank rank, InternalLinearBarcode internalLinearBarcode)
        {
            return new ProductUnitDetails(unit, conversionRate, rank, internalLinearBarcode);
        }

        private static InternalLinearBarcode CreateInternalLinearBarcode()
        {
            return InternalLinearBarcode.New("123456789");
        }
    }
}
