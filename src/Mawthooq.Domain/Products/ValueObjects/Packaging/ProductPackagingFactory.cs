using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Packaging
{
    public static class ProductPackagingFactory
    {
        public static ProductPackaging Create(
            PackagingTemplate template,
            int? totalUnitsPerBox,
            UnitOfMeasure? directUnit,
            int? stripsPerBox,
            int? unitsPerStrip,
            int? innerBoxesPerOuterBox,
            int? piecesPerInnerBox,
            UnitOfMeasure? smallUnit)
        {
            return template switch
            {
                PackagingTemplate.SingleUnit =>
                     SingleUnitPackaging.Create(
                        smallUnit ?? throw new ArgumentException("SmallUnit is required")),

                PackagingTemplate.BoxWithDirectUnits =>
                     BoxWithDirectUnitsPackaging.Create(
                        totalUnitsPerBox ?? throw new ArgumentException("TotalUnitsPerBox is required"),
                        directUnit ?? throw new ArgumentException("DirectUnit is required")),

                PackagingTemplate.BoxWithStripsAndUnits =>
                     BoxWithStripsPackaging.Create(
                        stripsPerBox ?? throw new ArgumentException("StripsPerBox is required"),
                        unitsPerStrip ?? throw new ArgumentException("UnitsPerStrip is required"),
                        smallUnit ?? throw new ArgumentException("SmallUnit is required")),

                PackagingTemplate.OuterBoxWithInnerBoxesAndPieces =>
                     OuterBoxPackaging.Create(
                        innerBoxesPerOuterBox ?? throw new ArgumentException("InnerBoxesPerOuterBox is required"),
                        piecesPerInnerBox ?? throw new ArgumentException("PiecesPerInnerBox is required"),
                        smallUnit ?? throw new ArgumentException("SmallUnit is required")),

                _ => throw new NotSupportedException($"Packaging template {template} is not supported.")
            };
        }
    }
}
