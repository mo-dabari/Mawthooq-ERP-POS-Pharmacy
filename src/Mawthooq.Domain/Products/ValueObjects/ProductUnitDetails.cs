using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.SharedKernel.Exceptions;

namespace Mawthooq.Domain.Products.ValueObjects
{
    public record class ProductUnitDetails
    {
        private readonly List<Barcode> _barcodes = new();
        public IReadOnlyList<Barcode> Barcodes => _barcodes.AsReadOnly();

        public UnitOfMeasure Unit { get; }
        public int ConversionRate { get; }
        public UnitRank Rank { get; }
        internal ProductUnitDetails(UnitOfMeasure unit, int conversionRate, UnitRank rank, InternalLinearBarcode internalBarcode)
        {
            ArgumentNullException.ThrowIfNull(internalBarcode, nameof(internalBarcode));

            if (!Enum.IsDefined(typeof(UnitOfMeasure), unit))
                throw new InvalidEnumArgumentException(nameof(unit), (int)unit, typeof(UnitOfMeasure));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(conversionRate, nameof(conversionRate));

            if (!Enum.IsDefined(typeof(UnitRank), rank))
                throw new InvalidEnumArgumentException(nameof(rank), (int)rank, typeof(UnitRank));

            Unit = unit;
            ConversionRate = conversionRate;
            Rank = rank;

            _barcodes.Add(internalBarcode);
        }

        private ProductUnitDetails(UnitOfMeasure unit, int conversionRate, UnitRank rank, IReadOnlyList<Barcode> barcodes)
        {
            Unit = unit;
            ConversionRate = conversionRate;
            Rank = rank;
            _barcodes = barcodes.ToList();
        }

        internal static ProductUnitDetails Reconstruct(UnitOfMeasure unit, int conversionRate, UnitRank rank, IReadOnlyList<Barcode> barcodes)
        {
            return new ProductUnitDetails(unit, conversionRate, rank, barcodes);
        }

        internal ProductUnitDetails AddCompanyBarcode(Barcode barcode)
        {
            ArgumentNullException.ThrowIfNull(barcode, nameof(barcode));

            if (barcode.Type == BarcodeType.Internal)
                throw new DomainRuleViolationException("Cannot add internal barcode manually.");

            if (_barcodes.Any(b => b.Value == barcode.Value))
                throw new DomainRuleViolationException("Barcode already exists for this unit.");

            var updatedBarcodes = new List<Barcode>(_barcodes) { barcode };
            return new ProductUnitDetails(Unit, ConversionRate, Rank, updatedBarcodes);
        }

        internal ProductUnitDetails RemoveCompanyBarcode(Barcode barcode)
        {
            ArgumentNullException.ThrowIfNull(barcode, nameof(barcode));

            if (barcode.Type == BarcodeType.Internal)
                throw new DomainRuleViolationException("Cannot add internal barcode manually.");

            var existingBarcode = _barcodes.FirstOrDefault(b => b.Value == barcode.Value);
            if (existingBarcode is null)
                throw new DomainRuleViolationException($"This barcode: {barcode.Value} does not exist.");

            var updatedBarcodes = new List<Barcode>(_barcodes);
            updatedBarcodes.Remove(existingBarcode);

            return new ProductUnitDetails(Unit, ConversionRate, Rank, updatedBarcodes);
        }
    }
}
