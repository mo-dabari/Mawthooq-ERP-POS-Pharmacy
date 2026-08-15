using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.Events;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace Mawthooq.Domain.Tests.Products
{
    public class ProductTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_CreateProduct_When_StateProductIsValid()
            {
                var sut = CreateProductValidWithoutClearingEvents();

                sut.TradeName.Should().Be("Panadol");
                sut.Status.Should().Be(Status.Active);
                sut.RequiresExpiryTracking.Should().BeTrue();
                sut.NearExpiryThresholdInDays.Should().Be(30);
                sut.CanBeReturnedIfOpened.Should().BeTrue();


                var firstPackagingVersions = sut.PackagingVersions[0];

                firstPackagingVersions.Units.Should().HaveCount(3);

                var box = firstPackagingVersions.Units.Single(x => x.Unit == UnitOfMeasure.Box);
                var stripe = firstPackagingVersions.Units.Single(x => x.Unit == UnitOfMeasure.Strip);
                var smallUnit = firstPackagingVersions.Units.Single(x => x.Unit == UnitOfMeasure.Disc);

                box.Barcodes[0].Value.Should().Be("1000");
                stripe.Barcodes[0].Value.Should().Be("1001");
                smallUnit.Barcodes[0].Value.Should().Be("1002");

                firstPackagingVersions.Status.Should().Be(Status.Active);
            }

            [Fact]
            public void Should_RaiseProductCreatedAndPackagingVersionAddedEvents_When_ProductIsCreated()
            {
                var sut = CreateProductValidWithoutClearingEvents();

                sut.DomainEvents.Should().HaveCount(2);
                sut.DomainEvents.Should().ContainSingle(e => e is ProductCreatedEvent);
                sut.DomainEvents.Should().ContainSingle(e => e is PackagingVersionAddedEvent);

                var createdEvent = (ProductCreatedEvent)sut.DomainEvents.Single(e => e is ProductCreatedEvent);
                createdEvent.ProductId.Should().Be(sut.ProductId);
                createdEvent.TradeName.Should().Be(sut.TradeName);
                createdEvent.Image.Should().Be(sut.Image);
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_ProductIDIsNull()
            {
                Action act = () => CreateProduct(
                    null!,
                    "Panadol",
                    CreateSKU(),
                    CreateCategoryID(),
                    CreateManufacturerID(),
                    CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(),
                    10001,
                    CreateImage(),
                    true,
                    true,
                    30);

                act.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData(null)]
            public void Should_ThrowIfNullOrWhiteSpace_When_NameProductIsInvalid(string? name)
            {
                Action sut = () => CreateProduct(
                    CreateProductID(),
                    name!,
                    CreateSKU(),
                    CreateCategoryID(),
                    CreateManufacturerID(),
                    CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(),
                    10000,
                    CreateImage(),
                    true,
                    true,
                    30);

                sut.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_CategoryIDIsNull()
            {
                Action act = () => CreateProduct(
                    CreateProductID(),
                    "Panadol",
                    CreateSKU(),
                    null!,
                    CreateManufacturerID(),
                    CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(),
                    10000,
                    CreateImage(),
                    true,
                    true,
                    30);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_ManufacturerIDIsNull()
            {
                Action act = () => CreateProduct(
                    CreateProductID(),
                    "Panadol",
                    CreateSKU(),
                    CreateCategoryID(),
                    null!,
                    CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(),
                    10000,
                    CreateImage(),
                    true,
                    true,
                    30);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_LocalAgentIDIsNull()
            {
                Action act = () => CreateProduct(
                    CreateProductID(),
                    "Panadol",
                    CreateSKU(),
                    CreateCategoryID(),
                    CreateManufacturerID(),
                    null!,
                    CreateProductBoxWithStripsPackaging(),
                    10000,
                    CreateImage(),
                    true,
                    true,
                    30);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_SKUIsNull()
            {
                Action act = () => CreateProduct(
                    CreateProductID(),
                    "Panadol",
                    null!,
                    CreateCategoryID(),
                    CreateManufacturerID(),
                    CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(),
                    10000,
                    CreateImage(),
                    true,
                    true,
                    30);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_ProductPackagingIsNull()
            {
                Action act = () => CreateProduct(
                    CreateProductID(),
                    "Panadol",
                    CreateSKU(),
                    CreateCategoryID(),
                    CreateManufacturerID(),
                    CreateLocalAgentID(),
                    null!,
                    10000,
                    CreateImage(),
                    true,
                    true,
                    30);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowArgumentOutOfRangeException_When_NearExpiryThresholdIsNegative()
            {
                Action act = () => CreateProduct(
                    CreateProductID(),
                    "Panadol",
                    CreateSKU(),
                    CreateCategoryID(),
                    CreateManufacturerID(),
                    CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(),
                    10000,
                    CreateImage(),
                    true,
                    true,
                    -1);

                act.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_RequiresExpiryTrackingIsTrue_AndThresholdIsZero()
            {
                Action act = () => CreateProduct(
                    CreateProductID(),
                    "Panadol",
                    CreateSKU(),
                    CreateCategoryID(),
                    CreateManufacturerID(),
                    CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(),
                    10000,
                    CreateImage(),
                    true,
                    true,
                    0);

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_AllowZeroThreshold_When_RequiresExpiryTrackingIsFalse()
            {
                var sut = CreateProduct(
                    CreateProductID(), "Panadol", CreateSKU(), CreateCategoryID(),
                    CreateManufacturerID(), CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(), 10000, CreateImage(), false, true, 0);

                sut.RequiresExpiryTracking.Should().BeFalse();
                sut.NearExpiryThresholdInDays.Should().Be(0);
            }
        }

        public class RehydrateBehavior
        {
            [Fact]
            public void Should_ReconstructProduct_WithoutRaisingDomainEvents()
            {
                var productId = CreateProductID();
                var packagingVersion = PackagingVersion.Create(
                    1, CreateProductBoxWithStripsPackaging().GetUnitDefinitions(), 1000);

                var sut = Product.Rehydrate(
                    productId, "Panadol", CreateSKU(), CreateCategoryID(),
                    CreateManufacturerID(), CreateLocalAgentID(), Status.Inactive,
                    true, true, 30, CreateImage(),
                    new List<PackagingVersion> { packagingVersion });

                sut.ProductId.Should().Be(productId);
                sut.PackagingVersions.Should().HaveCount(1);
                sut.DomainEvents.Should().BeEmpty();
            }

            [Fact]
            public void Should_PreserveTheGivenStatus_NotDefaultToActive()
            {
                var sut = Product.Rehydrate(
                    CreateProductID(), "Panadol", CreateSKU(), CreateCategoryID(),
                    CreateManufacturerID(), CreateLocalAgentID(), Status.Inactive,
                    true, true, 30, CreateImage(),
                    new List<PackagingVersion>());

                sut.Status.Should().Be(Status.Inactive);
            }
        }

        public class RenameProductBehavior
        {
            [Fact]
            public void Should_RenameProduct_And_RaiseProductRenamedEvent_When_NameIsValid()
            {
                Product sut = CreateProductValid();

                sut.Rename("Panadol Extra");

                sut.TradeName.Should().Be("Panadol Extra");

                var domainEvent = sut.DomainEvents
                    .Should()
                    .ContainSingle()
                    .Subject.Should().BeOfType<ProductRenamedEvent>()
                    .Subject;

                domainEvent.NewTradeName.Should().Be(sut.TradeName);
            }

            [Fact]
            public void Should_TrimName_When_RenamedWithSurroundingSpaces()
            {
                var sut = CreateProductValidWithoutClearingEvents();

                sut.Rename("  Panadol Extra  ");

                sut.TradeName.Should().Be("Panadol Extra");
            }

            [Fact]
            public void Should_NotRaiseEvent_When_NewNameIsSameAsCurrent()
            {
                var sut = CreateProductValid();

                sut.Rename(sut.TradeName);

                sut.DomainEvents.Should().BeEmpty();
            }

            [Theory]
            [InlineData("")]
            [InlineData("  ")]
            [InlineData(null)]
            public void Should_ThrowArgumentException_When_NewNameIsInvalid(string? newName)
            {
                Product sut = CreateProductValidWithoutClearingEvents();

                Action action = () => sut.Rename(newName!);

                action.Should().Throw<ArgumentException>();
            }
        }
        public class CreatePackagingVersionBehavior
        {
            [Fact]
            public void Should_CreateNewVersion_And_RaisePackagingVersionAddedEvent_When_Valid()
            {
                var sut = CreateProductValid();
                ProductPackaging productPackaging = BoxWithStripsPackaging.Create(3, 10, UnitOfMeasure.Capsule);
                int expectedVersion = sut.PackagingVersions.Last().Version + 1;

                sut.CreatePackagingVersion(productPackaging, 9000);

                sut.PackagingVersions.Should().HaveCount(2);

                var newVersion = sut.PackagingVersions.Last();

                newVersion.Version.Should().Be(expectedVersion);
                newVersion.Units.Should().HaveCount(3);

                var box = newVersion.Units.Single(x => x.Unit == UnitOfMeasure.Box);
                var stripe = newVersion.Units.Single(x => x.Unit == UnitOfMeasure.Strip);
                var capsule = newVersion.Units.Single(x => x.Unit == UnitOfMeasure.Capsule);

                box.ConversionRate.Should().Be(30);
                box.Barcodes[0].Value.Should().Be("9000");

                stripe.ConversionRate.Should().Be(10);
                stripe.Barcodes[0].Value.Should().Be("9001");

                capsule.ConversionRate.Should().Be(1);
                capsule.Barcodes[0].Value.Should().Be("9002");

                var domainEvent = sut.DomainEvents
                    .Should()
                    .ContainSingle()
                    .Subject.Should().BeOfType<PackagingVersionAddedEvent>()
                    .Subject;

                domainEvent.PackagingVersionId.Should().Be(newVersion.PackagingVersionId);
                domainEvent.Version.Should().Be(expectedVersion);
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_PackagingIsNull()
            {
                var sut = CreateProductValid();

                Action act = () => sut.CreatePackagingVersion(null!, 9000);

                act.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_ThrowArgumentOutOfRangeException_When_FirstReservedBarcodeIsInvalid(long invalidFirstReservedBarcodeInternal)
            {
                var sut = CreateProductValid();
                ProductPackaging productPackaging = BoxWithStripsPackaging.Create(3, 10, UnitOfMeasure.Capsule);

                Action act = () => sut.CreatePackagingVersion(productPackaging, invalidFirstReservedBarcodeInternal);

                act.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_PackagingVersionIsDublicated()
            {
                var sut = CreateProductValid();

                Action act = () => sut.CreatePackagingVersion(CreateProductBoxWithStripsPackaging(), 9000);

                act.Should().Throw<InvalidOperationException>();
            }
        }
        public class AddCompanyBarcodeToVersionBehavior
        {
            [Fact]
            public void Should_AddBarcode_And_RaiseCompanyBarcodeAddedEvent_When_Valid()
            {
                Product sut = CreateProductValid();

                var versionId = sut.PackagingVersions[0].PackagingVersionId;
                Barcode newBarcode = BarcodeEAN13Linear.New("1234567891234");

                sut.AddCompanyBarcodeToVersion(versionId, UnitOfMeasure.Box, newBarcode);

                var boxUnit = sut.PackagingVersions[0].Units.Single(u => u.Unit == UnitOfMeasure.Box);
                boxUnit.Barcodes.Should().HaveCount(2);
                boxUnit.Barcodes.Should().Contain(newBarcode);


                var domainEvent = sut.DomainEvents
                    .Should()
                    .ContainSingle()
                    .Subject.Should().BeOfType<CompanyBarcodeAddedEvent>()
                    .Subject;

                domainEvent.ProductId.Should().Be(sut.ProductId);
                domainEvent.PackagingVersionId.Should().Be(versionId);
                domainEvent.unit.Should().Be(UnitOfMeasure.Box);
                domainEvent.barcode.Should().Be(newBarcode);
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_PackagingVersionIdIsNull()
            {
                var sut = CreateProductValid();

                Action act = () => sut.AddCompanyBarcodeToVersion(null!, UnitOfMeasure.Box, BarcodeEAN13Linear.New("1234567891234"));

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowInvalidEnumArgumentException_When_UnitIsInvalid()
            {
                var sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.AddCompanyBarcodeToVersion(versionId, (UnitOfMeasure)999, BarcodeEAN13Linear.New("1234567891234"));

                act.Should().Throw<InvalidEnumArgumentException>();
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_BarcodeIsNull()
            {
                var sut = CreateProductValid();
                var PackagingVersionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.AddCompanyBarcodeToVersion(PackagingVersionId, UnitOfMeasure.Box, null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_AddedingBarcodeIsBarcodeRepeatedInProduct()
            {
                var sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;
                sut.AddCompanyBarcodeToVersion(versionId, UnitOfMeasure.Box, BarcodeEAN13Linear.New("1234567891234"));

                Action act = () => sut.AddCompanyBarcodeToVersion(versionId, UnitOfMeasure.Box, BarcodeEAN13Linear.New("1234567891234"));

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_BarcodeIsInternalType()
            {
                var sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.AddCompanyBarcodeToVersion(versionId, UnitOfMeasure.Box, InternalLinearBarcode.New("12345"));

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_AddedingBarcodeIsPackagingVersionIdNotExist()
            {
                var sut = CreateProductValid();
                var notFoundVersionId = PackagingVersionId.From(new Guid("11111111-1111-1111-1111-111111111111"));

                Action act = () => sut.AddCompanyBarcodeToVersion(notFoundVersionId, UnitOfMeasure.Box, InternalLinearBarcode.New("12345"));

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_UnitNotFoundInVersion()
            {
                var sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.AddCompanyBarcodeToVersion(versionId, UnitOfMeasure.Roll, BarcodeEAN13Linear.New("1234567891234"));

                act.Should().Throw<InvalidOperationException>();
            }
        }
        public class RemoveCompanyBarcodeBehavior
        {
            [Fact]
            public void Should_RemoveBarcode_And_RaiseCompanyBarcodeRemovedEvent_When_Valid()
            {
                Product sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;
                Barcode barcode = BarcodeEAN13Linear.New("1234567891234");

                sut.AddCompanyBarcodeToVersion(versionId, UnitOfMeasure.Box, barcode);
                sut.ClearDomainEvents();

                sut.RemoveCompanyBarcodeFromVersion(versionId, UnitOfMeasure.Box, barcode);

                var boxUnit = sut.PackagingVersions[0].Units.Single(u => u.Unit == UnitOfMeasure.Box);
                boxUnit.Barcodes.Should().NotContain(barcode);

                var domainEvent = sut.DomainEvents
                    .Should()
                    .ContainSingle()
                    .Subject.Should().BeOfType<CompanyBarcodeRemovedEvent>()
                    .Subject;

                domainEvent.PackagingVersionId.Should().Be(versionId);
                domainEvent.barcode.Should().Be(barcode);
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_PackagingVersionIdIsNull()
            {
                var sut = CreateProductValid();

                Action act = () => sut.RemoveCompanyBarcodeFromVersion(null!, UnitOfMeasure.Box, BarcodeEAN13Linear.New("1234567891234"));

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowInvalidEnumArgumentException_When_UnitOfMeasureIsInvalid()
            {
                var sut = CreateProductValid();
                var PackagingVersionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.RemoveCompanyBarcodeFromVersion(PackagingVersionId, (UnitOfMeasure)999, BarcodeEAN13Linear.New("1234567891234"));

                act.Should().Throw<InvalidEnumArgumentException>();
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_BarcodeIsNull()
            {
                var sut = CreateProductValid();
                var PackagingVersionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.RemoveCompanyBarcodeFromVersion(PackagingVersionId, UnitOfMeasure.Box, null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_PackagingVersionIsNotFound()
            {
                var sut = CreateProductValid();
                var notFoundPackagingVersionId = PackagingVersionId.From(new Guid(("11111111-1111-1111-1111-111111111111")));

                Action act = () => sut.RemoveCompanyBarcodeFromVersion(notFoundPackagingVersionId, UnitOfMeasure.Box, InternalLinearBarcode.New("12345"));

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_UnitOfMeasureIsNotFound()
            {
                var sut = CreateProductValid();
                var PackagingVersionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.RemoveCompanyBarcodeFromVersion(PackagingVersionId, UnitOfMeasure.Roll, BarcodeEAN13Linear.New("1234567891234"));

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_BarcodeIsNotFound()
            {
                var sut = CreateProductValid();
                var PackagingVersionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.RemoveCompanyBarcodeFromVersion(PackagingVersionId, UnitOfMeasure.Box, BarcodeEAN13Linear.New("1234567891234"));

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_BarcodeIsInternalType()
            {
                var sut = CreateProductValid();
                var PackagingVersionId = sut.PackagingVersions[0].PackagingVersionId;

                Action act = () => sut.RemoveCompanyBarcodeFromVersion(PackagingVersionId, UnitOfMeasure.Box, InternalLinearBarcode.New("12345"));

                act.Should().Throw<InvalidOperationException>();
            }
        }
        public class PackagingVersionStatusBehavior
        {
            [Fact]
            public void Should_Deactivate_And_RaisePackagingVersionStatusEvent()
            {
                var sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;

                sut.DeactivatePackagingVersions(versionId);

                sut.PackagingVersions[0].Status.Should().Be(Status.Inactive);

                var domainEvent = sut.DomainEvents
                    .Should()
                    .ContainSingle()
                    .Subject.Should().BeOfType<PackagingVersionStatusEvent>()
                    .Subject;

                domainEvent.PackagingVersionId.Should().Be(versionId);
                domainEvent.newStatus.Should().Be(Status.Inactive);
            }

            [Fact]
            public void Should_Activate_And_RaisePackagingVersionStatusEvent()
            {
                var sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;
                sut.DeactivatePackagingVersions(versionId);
                sut.ClearDomainEvents();

                sut.ActivatePackagingVersions(versionId);

                sut.PackagingVersions[0].Status.Should().Be(Status.Active);

                var domainEvent = sut.DomainEvents.Should().ContainSingle()
                    .Subject.Should().BeOfType<PackagingVersionStatusEvent>().Subject;

                domainEvent.newStatus.Should().Be(Status.Active);
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_ActivateWithNullId()
            {
                var sut = CreateProductValid();

                Action act = () => sut.ActivatePackagingVersions(null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_DeactivateWithNullId()
            {
                var sut = CreateProductValid();

                Action act = () => sut.DeactivatePackagingVersions(null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_ActivateVersionNotFound()
            {
                var sut = CreateProductValid();
                var notFoundPackagingVersionId = PackagingVersionId.From(new Guid("11111111-1111-1111-1111-111111111111"));

                Action act = () => sut.ActivatePackagingVersions(notFoundPackagingVersionId);

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_DeactivateVersionNotFound()
            {
                var sut = CreateProductValid();
                var notFoundPackagingVersionId = PackagingVersionId.From(new Guid("11111111-1111-1111-1111-111111111111"));

                Action act = () => sut.DeactivatePackagingVersions(notFoundPackagingVersionId);

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void Should_NotRaiseEvent_When_NewStatusIsActivate()
            {
                var sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;

                sut.ActivatePackagingVersions(versionId);
                sut.ClearDomainEvents();

                sut.ActivatePackagingVersions(versionId);

                sut.DomainEvents.Should().BeEmpty();
            }

             [Fact]
            public void Should_NotRaiseEvent_When_NewStatusIsDeactivate()
            {
                var sut = CreateProductValid();
                var versionId = sut.PackagingVersions[0].PackagingVersionId;

                sut.DeactivatePackagingVersions(versionId);
                sut.ClearDomainEvents();

                sut.DeactivatePackagingVersions(versionId);

                sut.DomainEvents.Should().BeEmpty();
            }

        }
        public class ChangeCategoryBehavior
        {
            [Fact]
            public void Should_UpdateCategory_And_RaiseProductCategoryChangedEvent_When_Different()
            {
                var sut = CreateProductValid();
                var newCategoryId = CategoryId.New();

                sut.ChangeCategory(newCategoryId);

                sut.CategoryId.Should().Be(newCategoryId);

                var domainEvent = sut.DomainEvents
                    .Should()
                    .ContainSingle()
                    .Subject.Should().BeOfType<ProductCategoryChangedEvent>()
                    .Subject;

                domainEvent.NewCategoryId.Should().Be(sut.CategoryId);
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_CategoryIdIsNull()
            {
                var sut = CreateProductValid();

                Action act = () => sut.ChangeCategory(null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_NotRaiseEvent_When_NewCategoryIdIsSame()
            {
                var sut = CreateProductValid();
                var sameCategoryId = sut.CategoryId;

                sut.ChangeCategory(sameCategoryId);

                sut.DomainEvents.Should().BeEmpty();
            }
        }
        public class ReturnPolicyBehavior
        {
            [Fact]
            public void Should_AllowReturn_And_RaiseProductReturnPolicyChangedEvent_When_CurrentlyDisallowed()
            {
                var sut = CreateProductValid();

                sut.DisallowReturnIfOpened();
                sut.ClearDomainEvents();

                sut.AllowReturnIfOpened();

                sut.CanBeReturnedIfOpened.Should().BeTrue();

                var domainEvent = sut.DomainEvents.Should().ContainSingle()
                     .Subject.Should().BeOfType<ProductReturnPolicyChangedEvent>().Subject;

                domainEvent.CanBeReturnedIfOpened.Should().BeTrue();
            }

            [Fact]
            public void Should_DisallowReturn_And_RaiseProductReturnPolicyChangedEvent_When_CurrentlyAllowed()
            {
                var sut = CreateProductValid();

                sut.DisallowReturnIfOpened();

                sut.CanBeReturnedIfOpened.Should().BeFalse();

                var domainEvent = sut.DomainEvents.Should().ContainSingle()
                    .Subject.Should().BeOfType<ProductReturnPolicyChangedEvent>().Subject;

                domainEvent.CanBeReturnedIfOpened.Should().BeFalse();
            }

            [Fact]
            public void Should_NotRaiseEvent_When_AllowReturnCalled_AndAlreadyAllowed()
            {
                var sut = CreateProductValid();

                sut.AllowReturnIfOpened();

                sut.DomainEvents.Should().BeEmpty();
            }

            [Fact]
            public void Should_NotRaiseEvent_When_DisallowReturnCalled_AndAlreadyDisallowed()
            {
                var sut = CreateProductValid();
                sut.DisallowReturnIfOpened();
                sut.ClearDomainEvents();

                sut.DisallowReturnIfOpened();

                sut.DomainEvents.Should().BeEmpty();
            }
        }
        public class ProductStatusBehavior
        {
            [Fact]
            public void Should_Deactivate_And_RaiseProductStatusChangedEvent()
            {
                var sut = CreateProductValid();

                sut.Deactivate();

                sut.Status.Should().Be(Status.Inactive);

                var domainEvent = sut.DomainEvents.Should().ContainSingle()
                    .Subject.Should().BeOfType<ProductStatusChangedEvent>().Subject;

                domainEvent.Status.Should().Be(Status.Inactive);
            }

            [Fact]
            public void Should_Activate_And_RaiseProductStatusChangedEvent()
            {
                var sut = CreateProductValid();
                sut.Deactivate();
                sut.ClearDomainEvents();

                sut.Activate();

                sut.Status.Should().Be(Status.Active);

                var domainEvent = sut.DomainEvents.Should().ContainSingle()
                    .Subject.Should().BeOfType<ProductStatusChangedEvent>().Subject;

                domainEvent.Status.Should().Be(Status.Active);
            }

            [Fact]
            public void Should_NotRaiseEvent_When_ActivateCalled_AndAlreadyActive()
            {
                var sut = CreateProductValid();

                sut.Activate();

                sut.DomainEvents.Should().BeEmpty();
            }

            [Fact]
            public void Should_NotRaiseEvent_When_DeactivateCalled_AndAlreadyInactive()
            {
                var sut = CreateProductValid();
                sut.Deactivate();
                sut.ClearDomainEvents();

                sut.Deactivate();

                sut.DomainEvents.Should().BeEmpty();
            }
        }
        public class ChangeNearExpiryThresholdBehavior
        {
            [Fact]
            public void Should_UpdateThreshold_And_RaiseEvent_When_Valid()
            {
                var sut = CreateProductValid();

                sut.ChangeNearExpiryThreshold(15);

                sut.RequiresExpiryTracking.Should().BeTrue();
                sut.NearExpiryThresholdInDays.Should().Be(15);

                var domainEvent = sut.DomainEvents.Should().ContainSingle()
                    .Subject.Should().BeOfType<ProductNearExpiryThresholdChangedEvent>().Subject;

                domainEvent.NearExpiryThresholdInDays.Should().Be(15);
            }

            [Fact]
            public void Should_NotRaiseEvent_When_ThresholdUnchanged()
            {
                var sut = CreateProductValid();

                sut.ChangeNearExpiryThreshold(sut.NearExpiryThresholdInDays);

                sut.DomainEvents.Should().BeEmpty();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_ThrowArgumentOutOfRangeException_When_ThresholdIsInvalid(int nearExpiryThresholdInDays)
            {
                var sut = CreateProductValid();

                Action act = () => sut.ChangeNearExpiryThreshold(nearExpiryThresholdInDays);

                act.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Should_ThrowInvalidOperationException_When_ProductDoesNotRequireExpiryTracking()
            {
                var sut = CreateProduct(
                    CreateProductID(),
                    "  Panadol  ",
                    CreateSKU(),
                    CreateCategoryID(),
                    CreateManufacturerID(),
                    CreateLocalAgentID(),
                    CreateProductBoxWithStripsPackaging(),
                    1000,
                    CreateImage(),
                    false,
                    true,
                    0);

                Action act = () => sut.ChangeNearExpiryThreshold(15);

                act.Should().Throw<InvalidOperationException>();
            }
        }
        public class ChangeImageBehavior
        {
            [Fact]
            public void Should_UpdateImage_And_RaiseProductImageChangedEvent_When_Different()
            {
                var sut = CreateProductValid();

                Image newImage = Image.Create("products/2/new.png", "new.png", "image/png");


                sut.ChangeImage(newImage);

                sut.Image.Should().Be(newImage);

                var domainEvent = sut.DomainEvents.Should().ContainSingle()
                    .Subject.Should().BeOfType<ProductImageChangedEvent>().Subject;

                domainEvent.Image.Should().Be(newImage);
            }

            [Fact]
            public void Should_ThrowArgumentNullException_When_ImageIsNull()
            {
                var sut = CreateProductValid();

                Action act = () => sut.ChangeImage(null!);

                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Should_NotRaiseEvent_When_NewImageIsSameAsCurrent()
            {
                var sut = CreateProductValid();

                sut.ChangeImage(sut.Image);

                sut.DomainEvents.Should().BeEmpty();
            }

        }
        private static Product CreateProduct(
            ProductId productId,
            string name,
            SKU sku,
            CategoryId categoryId,
            ManufacturerId manufacturerId,
            LocalAgentId localAgentId,
            ProductPackaging packaging,
            long firstReservedBarcode,
            Image image,
            bool requiresExpiryTracking,
            bool canBeReturnedIfOpened,
            int nearExpiryThresholdInDays
        )
        {
            return Product.Create(
                productId,
                name,
                sku,
                categoryId,
                manufacturerId,
                localAgentId,
                packaging,
                firstReservedBarcode,
                image,
                requiresExpiryTracking,
                canBeReturnedIfOpened,
                nearExpiryThresholdInDays
                );
        }

        /// <summary>
        /// Returns a valid product with domain events already cleared,
        /// so tests can assert only the events raised by the behavior under test.
        /// </summary>
        private static Product CreateProductValid()
        {
            var product = CreateProductValidWithoutClearingEvents();
            product.ClearDomainEvents();
            return product;
        }
        private static Product CreateProductValidWithoutClearingEvents()
        {
            return CreateProduct(
                CreateProductID(),
                "  Panadol  ",
                CreateSKU(),
                CreateCategoryID(),
                CreateManufacturerID(),
                CreateLocalAgentID(),
                CreateProductBoxWithStripsPackaging(),
                1000,
                CreateImage(),
                true,
                true,
                30);
        }
        private static ProductId CreateProductID() => ProductId.New();
        private static CategoryId CreateCategoryID() => CategoryId.New();
        private static ManufacturerId CreateManufacturerID() => ManufacturerId.New();
        private static LocalAgentId CreateLocalAgentID() => LocalAgentId.New();
        private static SKU CreateSKU() => SKU.Create("PROD-0001");

        private static Image CreateImage()
            => Image.Create("products/1/default.png", "default.png", "image/png");

        private static ProductPackaging CreateProductBoxWithStripsPackaging()
            => BoxWithStripsPackaging.Create(3, 10, UnitOfMeasure.Disc);
    }
}
