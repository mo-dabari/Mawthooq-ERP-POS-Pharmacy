using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Packaging
{
    public sealed record class ProductUnitDefinition
    {
        public UnitOfMeasure Unit { get; }
        public int ConversionRate { get; }
        public UnitRank Rank { get; }

        private ProductUnitDefinition(UnitOfMeasure unit, int conversionRate, UnitRank rank)
        {
            if (!Enum.IsDefined(typeof(UnitOfMeasure), unit))
                throw new InvalidEnumArgumentException(nameof(unit), (int)unit, typeof(UnitOfMeasure));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(conversionRate, nameof(conversionRate));

            if (!Enum.IsDefined(typeof(UnitRank), rank))
                throw new InvalidEnumArgumentException(nameof(rank), (int)rank, typeof(UnitRank));

            Unit = unit;
            ConversionRate = conversionRate;
            Rank = rank;
        }

        internal static ProductUnitDefinition Create(UnitOfMeasure unit, int conversionRate, UnitRank rank)
        {
            return new ProductUnitDefinition(unit, conversionRate, rank);
        }
    }
}
