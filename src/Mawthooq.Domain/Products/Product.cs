using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.Events;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.Exceptions;
using Mawthooq.Domain.SharedKernel.Primitives;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Products
{
    public class Product : AggregateRoot
    {
        public const int MinDays = 90;
        public const int MaxDays = 365;
        public const int NotTracked = 0;

        private readonly List<PackagingVersion> _packagingVersions = new();
        public IReadOnlyList<PackagingVersion> PackagingVersions => _packagingVersions.AsReadOnly();

        public ProductId ProductId { get; }
        public string TradeName { get; private set; }
        public SKU SKU { get; }
        public CategoryId CategoryId { get; private set; }
        public ManufacturerId ManufacturerId { get; private set; }
        public LocalAgentId LocalAgentId { get; private set; }
        public Image Image { get; private set; }
        public bool RequiresExpiryTracking { get; }
        public bool CanBeReturnedIfOpened { get; private set; }
        public int NearExpiryThresholdInDays { get; private set; }
        public Status Status { get; private set; }
        private Product(
            ProductId productId,
            string name,
            SKU sku,
            CategoryId categoryId,
            ManufacturerId manufacturerId,
            LocalAgentId localAgentId,
            Status status,
            bool requiresExpiryTracking,
            bool canBeReturnedIfOpened,
            int nearExpiryThresholdInDays,
            Image image
            )

        {
            ProductId = productId;
            TradeName = name;
            SKU = sku;
            CategoryId = categoryId;
            RequiresExpiryTracking = requiresExpiryTracking;
            CanBeReturnedIfOpened = canBeReturnedIfOpened;
            Status = status;
            ManufacturerId = manufacturerId;
            LocalAgentId = localAgentId;
            NearExpiryThresholdInDays = requiresExpiryTracking ? nearExpiryThresholdInDays : NotTracked;
            Image = image;
        }

        /// <summary>
        /// Creates a new product with an initial packaging version.
        /// </summary>
        internal static Product Create(
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

            ValidateArguments(
                productId,
                name,
                sku,
                categoryId,
                manufacturerId,
                localAgentId,
                packaging,
                nearExpiryThresholdInDays,
                requiresExpiryTracking,
                image);

            string trimmedName = name.Trim();
            Product product = new Product(
                productId,
                trimmedName,
                sku,
                categoryId,
                manufacturerId,
                localAgentId,
                Status.Active,
                requiresExpiryTracking,
                canBeReturnedIfOpened,
                nearExpiryThresholdInDays,
                image);

            product.RaiseDomainEvent(new ProductCreatedEvent(
                product.ProductId,
                product.TradeName,
                product.SKU,
                product.CategoryId,
                product.ManufacturerId,
                product.LocalAgentId,
                product.RequiresExpiryTracking,
                product.CanBeReturnedIfOpened,
                product.NearExpiryThresholdInDays,
                product.Image,
                DateTime.UtcNow));

            product.CreatePackagingVersion(packaging, firstReservedBarcode);

            return product;
        }

        /// <summary>
        /// Reconstructs a Product aggregate from persisted state.
        /// Does not raise domain events and does not run creation validation,
        /// since the data is assumed to already be valid (it came from storage).
        /// </summary>
        internal static Product Rehydrate(
            ProductId productId,
            string name,
            SKU sku,
            CategoryId categoryId,
            ManufacturerId manufacturerId,
            LocalAgentId localAgentId,
            Status status,
            bool requiresExpiryTracking,
            bool canBeReturnedIfOpened,
            int nearExpiryThresholdInDays,
            Image image,
            IEnumerable<PackagingVersion> packagingVersions)
        {
            ArgumentNullException.ThrowIfNull(packagingVersions, nameof(packagingVersions));

            Product product = new(
                productId,
                name,
                sku,
                categoryId,
                manufacturerId,
                localAgentId,
                status,
                requiresExpiryTracking,
                canBeReturnedIfOpened,
                nearExpiryThresholdInDays,
                image);

            product._packagingVersions.AddRange(packagingVersions);
            return product;
        }
        internal void CreatePackagingVersion(ProductPackaging packaging, long firstReservedBarcode)
        {
            ArgumentNullException.ThrowIfNull(packaging, nameof(packaging));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(firstReservedBarcode, nameof(firstReservedBarcode));

            var newDefinitions = packaging.GetUnitDefinitions().OrderBy(d => d.Rank).ToList();

            foreach (var existingVersion in _packagingVersions)
            {
                if (existingVersion.Units.Count != newDefinitions.Count)
                    continue;

                var existingUnitsSorted = existingVersion.Units.OrderBy(u => u.Rank).ToList();
                bool isIdentical = true;

                for (int i = 0; i < newDefinitions.Count; i++)
                {
                    var def = newDefinitions[i];
                    var currentUnit = existingUnitsSorted[i];

                    if (currentUnit.Unit != def.Unit ||
                        currentUnit.ConversionRate != def.ConversionRate ||
                        currentUnit.Rank != def.Rank)
                    {
                        isIdentical = false;
                        break;
                    }
                }

                if (isIdentical)
                    throw new DomainRuleViolationException(
                        @$"A packaging version with the exact same unit configuration
                        (Units, Conversion Rates, and Ranks) already exists for this product. Duplicate configurations are not allowed.");
            }

            int version = _packagingVersions.Count > 0 ? _packagingVersions.Max(v => v.Version) + 1 : 1;

            var packagingVersion = PackagingVersion.Create(
                version,
                packaging.GetUnitDefinitions(),
                firstReservedBarcode);

            _packagingVersions.Add(packagingVersion);

            RaiseDomainEvent(
                new PackagingVersionAddedEvent(
                    ProductId,
                    packagingVersion.PackagingVersionId,
                    version,
                    DateTime.UtcNow));
        }
        public void AddCompanyBarcodeToVersion(PackagingVersionId versionId, UnitOfMeasure unit, Barcode barcode)
        {
            ArgumentNullException.ThrowIfNull(versionId, nameof(versionId));

            if (!Enum.IsDefined(typeof(UnitOfMeasure), unit))
                throw new InvalidEnumArgumentException(nameof(unit), (int)unit, typeof(UnitOfMeasure));

            ArgumentNullException.ThrowIfNull(barcode, nameof(barcode));

            if (barcode.Type == BarcodeType.Internal)
                throw new DomainRuleViolationException("cannot adding an internal-type barcode");

            var version = _packagingVersions.SingleOrDefault(v => v.PackagingVersionId == versionId);
            if (version is null)
                throw new DomainRuleViolationException("Packaging version not found.");

            var existUnit = version.Units.Any(u => u.Unit == unit);
            if (!existUnit)
                throw new DomainRuleViolationException($"UnitOfMeasure: {unit} Not found");

            bool isBarcodeRepeatedInProduct = _packagingVersions
                    .SelectMany(v => v.Units)
                    .SelectMany(u => u.Barcodes)
                    .Any(b => b.Value == barcode.Value);

            if (isBarcodeRepeatedInProduct)
                throw new DomainRuleViolationException($"The barcode '{barcode.Value}' is already assigned to another unit within this product.");

            version.AddCompanyBarcodeToUnit(unit, barcode);

            RaiseDomainEvent(
                new CompanyBarcodeAddedEvent(
                    ProductId,
                    versionId,
                    unit,
                    barcode,
                    DateTime.UtcNow));
        }
        public void RemoveCompanyBarcodeFromVersion(PackagingVersionId versionId, UnitOfMeasure unit, Barcode barcode)
        {
            ArgumentNullException.ThrowIfNull(versionId, nameof(versionId));

            if (!Enum.IsDefined(typeof(UnitOfMeasure), unit))
                throw new InvalidEnumArgumentException(nameof(unit), (int)unit, typeof(UnitOfMeasure));

            ArgumentNullException.ThrowIfNull(barcode, nameof(barcode));

            if (barcode.Type == BarcodeType.Internal)
                throw new DomainRuleViolationException("Cannot Removing Barcode From Type Internal");

            var version = _packagingVersions.FirstOrDefault(v => v.PackagingVersionId == versionId);
            if (version is null)
                throw new DomainRuleViolationException("Packaging version not found.");

            var existUnit = version.Units.FirstOrDefault(u => u.Unit == unit);
            if (existUnit is null)
                throw new DomainRuleViolationException($"UnitOfMeasure: {unit} Not found");

            bool isBarcode = existUnit.Barcodes.Any(b => b.Equals(barcode));

            if (!isBarcode)
                throw new DomainRuleViolationException($"Barcode : {barcode} Not Found");

            version.RemoveCompanyBarcodeFromUnit(unit, barcode);

            RaiseDomainEvent(
                new CompanyBarcodeRemovedEvent(
                        ProductId,
                        versionId,
                        unit,
                        barcode,
                        DateTime.UtcNow));
        }
        public void ActivatePackagingVersions(PackagingVersionId packagingVersionId)
        {
            ArgumentNullException.ThrowIfNull(packagingVersionId, nameof(packagingVersionId));

            var existPackagingVersions = _packagingVersions.SingleOrDefault(pv => pv.PackagingVersionId == packagingVersionId);

            if (existPackagingVersions is null)
                throw new DomainRuleViolationException($"The Packaging Version Not Exists");

            if (existPackagingVersions.Status == Status.Active) return;

            existPackagingVersions.ActivatePackagingVersion();

            RaiseDomainEvent(
                new PackagingVersionStatusEvent(
                        ProductId,
                        packagingVersionId,
                        existPackagingVersions.Status,
                        DateTime.UtcNow));
        }
        public void DeactivatePackagingVersions(PackagingVersionId packagingVersionId)
        {
            ArgumentNullException.ThrowIfNull(packagingVersionId, nameof(packagingVersionId));

            var existPackagingVersions = _packagingVersions.SingleOrDefault(pv => pv.PackagingVersionId == packagingVersionId);

            if (existPackagingVersions is null)
                throw new DomainRuleViolationException($"The Packaging Version Not Exists");

            if (existPackagingVersions.Status == Status.Inactive) return;

            existPackagingVersions.DeactivatePackagingVersion();

            RaiseDomainEvent(
                 new PackagingVersionStatusEvent(
                         ProductId,
                         packagingVersionId,
                         existPackagingVersions.Status,
                         DateTime.UtcNow));
        }
        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName, nameof(newName));

            newName = newName.Trim();

            if (TradeName.Equals(newName, StringComparison.OrdinalIgnoreCase))
                return;

            TradeName = newName;

            RaiseDomainEvent(new ProductRenamedEvent(ProductId, TradeName, DateTime.UtcNow));
        }
        public void ChangeCategory(CategoryId categoryId)
        {
            ArgumentNullException.ThrowIfNull(categoryId, nameof(categoryId));

            if (CategoryId.Equals(categoryId))
                return;

            CategoryId = categoryId;

            RaiseDomainEvent(new ProductCategoryChangedEvent(ProductId, categoryId, DateTime.UtcNow));
        }
        public void AllowReturnIfOpened()
        {
            if (CanBeReturnedIfOpened) return;

            CanBeReturnedIfOpened = true;

            RaiseDomainEvent(new ProductReturnPolicyChangedEvent(ProductId, CanBeReturnedIfOpened, DateTime.UtcNow));
        }
        public void DisallowReturnIfOpened()
        {
            if (!CanBeReturnedIfOpened) return;

            CanBeReturnedIfOpened = false;

            RaiseDomainEvent(new ProductReturnPolicyChangedEvent(ProductId, CanBeReturnedIfOpened, DateTime.UtcNow));
        }
        public void Activate()
        {
            if (Status.Equals(Status.Active))
                return;
            Status = Status.Active;

            RaiseDomainEvent(new ProductStatusChangedEvent(ProductId, Status, DateTime.UtcNow));
        }
        public void Deactivate()
        {
            if (Status.Equals(Status.Inactive))
                return;

            Status = Status.Inactive;

            RaiseDomainEvent(new ProductStatusChangedEvent(ProductId, Status, DateTime.UtcNow));
        }
        internal static void EnsureInRange(int days)
        {
            if (days < MinDays || days > MaxDays)
                throw new DomainRuleViolationException($" {nameof(NearExpiryThresholdInDays)} must be between {MinDays} and {MaxDays} days.");
        }
        public void ChangeNearExpiryThreshold(int thresholdInDays)
        {
            if (!RequiresExpiryTracking)
                throw new DomainRuleViolationException("Cannot change near expiry threshold for products that do not require expiry tracking.");

            EnsureInRange(thresholdInDays);

            if (NearExpiryThresholdInDays == thresholdInDays)
                return;

            NearExpiryThresholdInDays = thresholdInDays;

            RaiseDomainEvent(new ProductNearExpiryThresholdChangedEvent(ProductId, NearExpiryThresholdInDays, DateTime.UtcNow));
        }
        public void ChangeImage(Image image)
        {
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            if (Image.Equals(image))
                return;

            Image = image;

            RaiseDomainEvent(new ProductImageChangedEvent(ProductId, Image, DateTime.UtcNow));
        }

        public ExpiryStatus GetExpiryStatus(DateOnly expiryDate, DateOnly today)
        {
            if (!RequiresExpiryTracking)
                return ExpiryStatus.NotTracked;

            if (expiryDate <= today)
                return ExpiryStatus.Expired;

            if (expiryDate <= today.AddDays(NearExpiryThresholdInDays))
                return ExpiryStatus.NearExpiry;

            return ExpiryStatus.Valid;
        }

        private static void ValidateArguments(
            ProductId productId,
            string name,
            SKU sku,
            CategoryId categoryId,
            ManufacturerId manufacturerId,
            LocalAgentId localAgentId,
            ProductPackaging packaging,
            int nearExpiryThresholdInDays,
            bool requiresExpiryTracking,
            Image image)
        {
            ArgumentNullException.ThrowIfNull(productId, nameof(productId));
            ArgumentNullException.ThrowIfNull(sku, nameof(sku));
            ArgumentNullException.ThrowIfNull(packaging, nameof(packaging));
            ArgumentNullException.ThrowIfNull(categoryId, nameof(categoryId));
            ArgumentNullException.ThrowIfNull(manufacturerId, nameof(manufacturerId));
            ArgumentNullException.ThrowIfNull(localAgentId, nameof(localAgentId));
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            if (requiresExpiryTracking)
                EnsureInRange(nearExpiryThresholdInDays);
        }
    }
}
