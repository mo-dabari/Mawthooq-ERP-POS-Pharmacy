using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Packaging
{
    public record BoxWithDirectUnitsPackaging : ProductPackaging
    {
        public int TotalUnitsPerBox { get; }
        public UnitOfMeasure DirectUnit { get;}

        private BoxWithDirectUnitsPackaging(int totalUnitsPerBox, UnitOfMeasure directUnit)
            : base(PackagingTemplate.BoxWithDirectUnits)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalUnitsPerBox, nameof(totalUnitsPerBox));

            if (!Enum.IsDefined(typeof(UnitOfMeasure), directUnit))
                throw new InvalidEnumArgumentException(nameof(directUnit), (int)directUnit, typeof(UnitOfMeasure));

            TotalUnitsPerBox = totalUnitsPerBox;
            DirectUnit = directUnit;
        }

        internal static BoxWithDirectUnitsPackaging Create(int totalUnitsPerBox, UnitOfMeasure directUnit)
        {
            return new BoxWithDirectUnitsPackaging(totalUnitsPerBox,directUnit);
        }

        internal override IEnumerable<ProductUnitDefinition> GetUnitDefinitions()
        {
            return
            [
                ProductUnitDefinition.Create(UnitOfMeasure.Box, TotalUnitsPerBox, UnitRank.Primary),
                ProductUnitDefinition.Create(DirectUnit, 1, UnitRank.Tertiary)
            ];
        }
    }
}
