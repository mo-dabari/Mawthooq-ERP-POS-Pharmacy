using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Domain.Products.Entities
{
    public class PackagingVersion
    {
        private readonly List<ProductUnitDetails> _units = new();
        public PackagingVersionId PackagingVersionId { get; }
        public int Version { get; }
        public Status Status { get; private set; }
        public IReadOnlyList<ProductUnitDetails> Units => _units.AsReadOnly();

        private PackagingVersion(PackagingVersionId packagingVersionId, int version, Status status)
        {
            PackagingVersionId = packagingVersionId;
            Version = version;
            Status = status;
        }

        private PackagingVersion(PackagingVersionId packagingVersionId, int version, Status status, List<ProductUnitDetails> units)
        : this(packagingVersionId, version, status)
        {
            _units = units;
        }

        internal static PackagingVersion Rehydrate(PackagingVersionId packagingVersionId, int version, Status status, List<ProductUnitDetails> units)
        {
            return new PackagingVersion(packagingVersionId, version, status, units);
        }

        internal static PackagingVersion Create(int version, IEnumerable<ProductUnitDefinition> definitions, long firstReservedBarcode)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(version, nameof(version));
            ArgumentNullException.ThrowIfNull(definitions, nameof(definitions));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(firstReservedBarcode, nameof(firstReservedBarcode));

            PackagingVersion packagingVersion = new PackagingVersion(
                PackagingVersionId.New(),
                version,
                Status.Active
            );

            packagingVersion.CreateUnits(definitions.OrderBy(d => d.Rank), firstReservedBarcode);

            return packagingVersion;
        }



        private void CreateUnits(IEnumerable<ProductUnitDefinition> sortedDefinitions, long firstReservedBarcode)
        {
            ArgumentNullException.ThrowIfNull(sortedDefinitions, nameof(sortedDefinitions));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(firstReservedBarcode, nameof(firstReservedBarcode));

            long currentBarcode = firstReservedBarcode;

            foreach (var unitDef in sortedDefinitions)
            {
                var internalBarcode = InternalLinearBarcode.New(currentBarcode.ToString());

                if (_units.Any(u => u.Unit == unitDef.Unit))
                    throw new InvalidOperationException($"This Unit: {unitDef.Unit} Already Exists");

                if (_units.Any(u => u.Rank == unitDef.Rank))
                    throw new InvalidOperationException($"Cannot Add Level. Rank {unitDef.Rank} Already Exists");

                var unit = new ProductUnitDetails(
                    unitDef.Unit,
                    unitDef.ConversionRate,
                    unitDef.Rank,
                    internalBarcode
                );

                _units.Add(unit);
                currentBarcode++;
            }
        }

        internal void AddCompanyBarcodeToUnit(UnitOfMeasure unit, Barcode barcode)
        {
            var targetUnit = _units.FirstOrDefault(u => u.Unit == unit);
            if (targetUnit is null)
                throw new InvalidOperationException($"Unit {unit} not found in this packaging version.");

            int index = _units.IndexOf(targetUnit);

            _units[index] = targetUnit.AddCompanyBarcode(barcode);
        }

        internal void RemoveCompanyBarcodeFromUnit(UnitOfMeasure unit, Barcode barcode)
        {
            var targetUnit = _units.FirstOrDefault(u => u.Unit == unit);
            if (targetUnit is null)
                throw new InvalidOperationException($"Unit {unit} not found in this packaging version.");

            int index = _units.IndexOf(targetUnit);

            _units[index] = targetUnit.RemoveCompanyBarcode(barcode);
        }

        internal void ActivatePackagingVersion()
        {
            if (Status.Active == Status)
                return;

            Status = Status.Active;
        }

        internal void DeactivatePackagingVersion()
        {
            if (Status.Inactive == Status)
                return;

            Status = Status.Inactive;
        }
    }
}
