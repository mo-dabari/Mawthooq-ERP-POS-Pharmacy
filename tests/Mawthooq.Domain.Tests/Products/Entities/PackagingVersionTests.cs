using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Mawthooq.Domain.SharedKernel.Enums;
using Xunit;

namespace Mawthooq.Domain.Tests.Products.Entities
{
    public class PackagingVersionTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_CreatePackagingVersion_When_StatePackagingVersionIsValid()
            {
                var sut = CreatePackagingVersionIsValid();

                sut.PackagingVersionId.Should().NotBeNull();
                sut.PackagingVersionId.Value.Should().NotBe(Guid.Empty);
                sut.Version.Should().Be(1);
                sut.Status.Should().Be(Status.Active);

                sut.Units.Should().HaveCount(3);

                var units = sut.Units;

                units[0].Unit.Should().Be(UnitOfMeasure.Box);
                units[0].ConversionRate.Should().Be(100);
                units[0].Rank.Should().Be(UnitRank.Primary);
                units[0].Barcodes[0].Value.Should().Be("10000001");

                units[1].Unit.Should().Be(UnitOfMeasure.Strip);
                units[1].ConversionRate.Should().Be(10);
                units[1].Rank.Should().Be(UnitRank.Secondary);
                units[1].Barcodes[0].Value.Should().Be("10000002");

                units[2].Unit.Should().Be(UnitOfMeasure.Disc);
                units[2].ConversionRate.Should().Be(1);
                units[2].Rank.Should().Be(UnitRank.Tertiary);
                units[2].Barcodes[0].Value.Should().Be("10000003");
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_Throw_ArgumentOutOfRangeException_When_VersionIsInvalid(int version)
            {
                Action act = () => CreatePackagingVersion(version, CreateProductPackaging().GetUnitDefinitions(), 1000001);

                act.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Should_Throw_ArgumentNullException_When_DefinitionsIsNull()
            {
                Action act = () => CreatePackagingVersion(1, null!, 1000001);

                act.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_Throw_ArgumentOutOfRangeException_When_FirstReservedBarcodeIsInvalid(int invalidBarcode)
            {
                Action act = () => CreatePackagingVersion(1, CreateProductPackaging().GetUnitDefinitions(), invalidBarcode);

                act.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Should_Throw_InvalidOperationException_When_DuplicateUnitPassed()
            {
                var duplicateUnits = new List<ProductUnitDefinition>
                {
                    ProductUnitDefinition.Create(UnitOfMeasure.Box, 10, UnitRank.Primary),
                    ProductUnitDefinition.Create(UnitOfMeasure.Box, 1, UnitRank.Secondary)
                };

                Action act = () => CreatePackagingVersion(1, duplicateUnits, 10000001);

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_Throw_InvalidOperationException_When_DuplicateRankPassed()
            {
                var duplicateUnits = new List<ProductUnitDefinition>
                {
                    ProductUnitDefinition.Create(UnitOfMeasure.Box, 10, UnitRank.Primary),
                    ProductUnitDefinition.Create(UnitOfMeasure.Strip, 1, UnitRank.Primary)
                };

                Action act = () => CreatePackagingVersion(1, duplicateUnits, 10000001);

                act.Should().Throw<InvalidOperationException>();
            }
        }

        public class CompanyBarcodeBehavior
        {
            [Fact]
            public void Should_AddCompanyBarcodeToUnit_When_UnitAndBarcodeIsValid()
            {
                var findUnit = UnitOfMeasure.Box;
                var validCompanyBarcode = BarcodeEAN13Linear.New("1234567891234");
                var sut = CreatePackagingVersionIsValid();
                var unitBeforeAddBarcode = sut.Units.FirstOrDefault(u => u.Unit == findUnit);
                unitBeforeAddBarcode.Should().NotBeNull();

                sut.AddCompanyBarcodeToUnit(findUnit, validCompanyBarcode);

                var unitAfterAddBarcode = sut.Units.FirstOrDefault(u => u.Unit == findUnit);
                unitAfterAddBarcode.Should().NotBeNull();

                unitAfterAddBarcode.Should().NotBeSameAs(unitBeforeAddBarcode);

                unitAfterAddBarcode.Unit.Should().Be(unitBeforeAddBarcode.Unit);
                unitAfterAddBarcode.ConversionRate.Should().Be(unitBeforeAddBarcode.ConversionRate);
                unitAfterAddBarcode.Rank.Should().Be(unitBeforeAddBarcode.Rank);

                unitAfterAddBarcode.Barcodes.Should().Contain(validCompanyBarcode);
                unitAfterAddBarcode.Barcodes.Count.Should().Be(unitBeforeAddBarcode.Barcodes.Count + 1);
            }

            [Fact]
            public void Should_RemoveCompanyBarcodeToUnit_When_UnitAndBarcodeIsValid()
            {
                var findUnit = UnitOfMeasure.Box;
                var validCompanyBarcode = BarcodeEAN13Linear.New("1234567891234");
                var sut = CreatePackagingVersionIsValid();
                var unitBeforeRemoveBarcode = sut.Units.FirstOrDefault(u => u.Unit == findUnit);
                unitBeforeRemoveBarcode.Should().NotBeNull();

                sut.AddCompanyBarcodeToUnit(findUnit, validCompanyBarcode);
                sut.RemoveCompanyBarcodeFromUnit(findUnit, validCompanyBarcode);

                var unitAfterRemoveBarcode = sut.Units.FirstOrDefault(u => u.Unit == findUnit);
                unitAfterRemoveBarcode.Should().NotBeNull();

                unitAfterRemoveBarcode.Should().NotBeSameAs(unitBeforeRemoveBarcode);

                unitAfterRemoveBarcode.Unit.Should().Be(unitBeforeRemoveBarcode.Unit);
                unitAfterRemoveBarcode.ConversionRate.Should().Be(unitBeforeRemoveBarcode.ConversionRate);
                unitAfterRemoveBarcode.Rank.Should().Be(unitBeforeRemoveBarcode.Rank);

                unitAfterRemoveBarcode.Barcodes.Should().NotContain(validCompanyBarcode);
                unitAfterRemoveBarcode.Barcodes.Count.Should().Be(1);
            }

            [Fact]
            public void Should_Throw_InvalidOperationException_When_AddCompanyBarcodeToUnitDoesNotExist()
            {
                var notExistUnit = UnitOfMeasure.Roll;
                var validCompanyBarcode = BarcodeEAN13Linear.New("1234567891234");
                var sut = CreatePackagingVersionIsValid();

                Action act = () => sut.AddCompanyBarcodeToUnit(notExistUnit, validCompanyBarcode);

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_Throw_InvalidOperationException_When_RemoveCompanyBarcodeToUnitDoesNotExist()
            {
                var notExistUnit = UnitOfMeasure.Roll;
                var validCompanyBarcode = BarcodeEAN13Linear.New("1234567891234");
                var sut = CreatePackagingVersionIsValid();

                Action act = () => sut.RemoveCompanyBarcodeFromUnit(notExistUnit, validCompanyBarcode);

                act.Should().Throw<InvalidOperationException>();
            }
        }

        public class StatusPackagingVersionBehavior
        {
            [Fact]
            public void Should_ActivateStatus_When_PackagingVersionIsActivatedFromDeactivate()
            {
                var sut = CreatePackagingVersionIsValid();

                sut.DeactivatePackagingVersion();

                sut.ActivatePackagingVersion();

                sut.Status.Should().Be(Status.Active);
            }

            [Fact]
            public void Should_DeactivateStatus_When_PackagingVersionIsActivated()
            {
                var sut = CreatePackagingVersionIsValid();

                sut.DeactivatePackagingVersion();

                sut.Status.Should().Be(Status.Inactive);
            }

        }

        private static PackagingVersion CreatePackagingVersionIsValid()
        {
            int version = 1;
            long firstBarcode = 10000001;

            var definitions = CreateProductPackaging();
            return  PackagingVersion.Create(version, definitions.GetUnitDefinitions(), firstBarcode);
        }

        private static PackagingVersion CreatePackagingVersion(int version, IEnumerable<ProductUnitDefinition> definitions, long firstReservedBarcode)
        {
            return  PackagingVersion.Create(version, definitions, firstReservedBarcode);
        }

        private static ProductPackaging CreateProductPackaging()
        {
            return BoxWithStripsPackaging.Create(10, 10, UnitOfMeasure.Disc);
        }
    }

}
