using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Packaging
{
    public record SingleUnitPackaging : ProductPackaging
    {
        public UnitOfMeasure SingleUnit { get; }

        private SingleUnitPackaging(UnitOfMeasure singleUnit)
            : base(PackagingTemplate.SingleUnit)
        {
            if (!Enum.IsDefined(typeof(UnitOfMeasure), singleUnit))
                throw new InvalidEnumArgumentException(nameof(singleUnit), (int)singleUnit, typeof(UnitOfMeasure));

            SingleUnit = singleUnit;
        }

        internal static SingleUnitPackaging Create(UnitOfMeasure singleUnit)
        {
            return new SingleUnitPackaging(singleUnit);
        }

        internal override IEnumerable<ProductUnitDefinition> GetUnitDefinitions()
        {
            return
            [
                ProductUnitDefinition.Create(SingleUnit, 1, UnitRank.Tertiary)
            ];
        }
    }
}
