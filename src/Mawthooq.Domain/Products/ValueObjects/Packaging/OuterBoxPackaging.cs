using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Packaging
{
    internal record OuterBoxPackaging : ProductPackaging
    {
        public int InnerBoxesPerOuterBox { get; }
        public int PiecesPerInnerBox { get; }
        public UnitOfMeasure SmallUnit { get; }

        public OuterBoxPackaging(int innerBoxesPerOuterBox, int piecesPerInnerBox, UnitOfMeasure smallUnit)
            : base(PackagingTemplate.OuterBoxWithInnerBoxesAndPieces)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(innerBoxesPerOuterBox, nameof(innerBoxesPerOuterBox));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(piecesPerInnerBox, nameof(piecesPerInnerBox));

            if (!Enum.IsDefined(typeof(UnitOfMeasure), smallUnit))
                throw new InvalidEnumArgumentException(nameof(smallUnit), (int)smallUnit, typeof(UnitOfMeasure));

            InnerBoxesPerOuterBox = innerBoxesPerOuterBox;
            PiecesPerInnerBox = piecesPerInnerBox;
            SmallUnit = smallUnit;
        }

        internal static OuterBoxPackaging Create(int innerBoxesPerOuterBox, int piecesPerInnerBox, UnitOfMeasure smallUnit)
        {
            return new OuterBoxPackaging(innerBoxesPerOuterBox, piecesPerInnerBox, smallUnit);
        }
        internal override IEnumerable<ProductUnitDefinition> GetUnitDefinitions()
        {
            return
            [
                ProductUnitDefinition.Create(UnitOfMeasure.OuterBox, InnerBoxesPerOuterBox * PiecesPerInnerBox, UnitRank.Primary),
                ProductUnitDefinition.Create(UnitOfMeasure.Box, PiecesPerInnerBox, UnitRank.Secondary),
                ProductUnitDefinition.Create(SmallUnit, 1 , UnitRank.Tertiary),
            ];
        }
    }
}
