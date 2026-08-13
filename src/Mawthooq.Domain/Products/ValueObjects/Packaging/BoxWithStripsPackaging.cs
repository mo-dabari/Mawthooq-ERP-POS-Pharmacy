using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Packaging
{
    internal record BoxWithStripsPackaging : ProductPackaging
    {
        public int StripsPerBox { get; }
        public int TabletsPerStrip { get; }
        public UnitOfMeasure SmallUnit { get; }

        private BoxWithStripsPackaging(int stripsPerBox, int tabletsPerStrip, UnitOfMeasure smallUnit)
            : base(PackagingTemplate.BoxWithStripsAndUnits)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stripsPerBox, nameof(stripsPerBox));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(tabletsPerStrip, nameof(tabletsPerStrip));

            if (!Enum.IsDefined(typeof(UnitOfMeasure), smallUnit))
                throw new InvalidEnumArgumentException(nameof(smallUnit), (int)smallUnit, typeof(UnitOfMeasure));

            StripsPerBox = stripsPerBox;
            TabletsPerStrip = tabletsPerStrip;
            SmallUnit = smallUnit;
        }

        internal static BoxWithStripsPackaging Create(int stripsPerBox, int TabletsPerStrip, UnitOfMeasure smallUnit)
        {
            return new BoxWithStripsPackaging(stripsPerBox, TabletsPerStrip, smallUnit);
        }

        internal override IEnumerable<ProductUnitDefinition> GetUnitDefinitions()
        {
            return
            [
                ProductUnitDefinition.Create(UnitOfMeasure.Box, StripsPerBox * TabletsPerStrip , UnitRank.Primary),
                ProductUnitDefinition.Create(UnitOfMeasure.Strip, TabletsPerStrip, UnitRank.Secondary),
                ProductUnitDefinition.Create(SmallUnit, 1 , UnitRank.Tertiary),
            ];
        }
    }
}
