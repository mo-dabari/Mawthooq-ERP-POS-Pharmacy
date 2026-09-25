using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.Validation
{
    internal static class PackagingInformationValidator
    {
        public static void ValidateAndCollect(PackagingTemplate template, PackagingInformation packaging, List<string> errors)
        {
            switch (template)
            {
                case PackagingTemplate.SingleUnit:
                    EnumValidator.ValidateAndCollect(packaging.SmallUnit, nameof(packaging.SmallUnit), errors);
                    break;

                case PackagingTemplate.BoxWithDirectUnits:
                    NumberValidator.ValidatePositive(packaging.TotalUnitsPerBox, nameof(packaging.TotalUnitsPerBox), errors);
                    EnumValidator.ValidateAndCollect(packaging.DirectUnit, nameof(packaging.DirectUnit), errors);
                    break;


                case PackagingTemplate.BoxWithStripsAndUnits:
                    NumberValidator.ValidatePositive(packaging.StripsPerBox, nameof(packaging.StripsPerBox), errors);
                    NumberValidator.ValidatePositive(packaging.UnitsPerStrip, nameof(packaging.UnitsPerStrip), errors);
                    EnumValidator.ValidateAndCollect(packaging.SmallUnit, nameof(packaging.SmallUnit), errors);
                    break;

                case PackagingTemplate.OuterBoxWithInnerBoxesAndPieces:
                    NumberValidator.ValidatePositive(packaging.InnerBoxesPerOuterBox, nameof(packaging.InnerBoxesPerOuterBox), errors);
                    NumberValidator.ValidatePositive(packaging.PiecesPerInnerBox, nameof(packaging.PiecesPerInnerBox), errors);
                    EnumValidator.ValidateAndCollect(packaging.SmallUnit, nameof(packaging.SmallUnit), errors);
                    break;

                default:
                    errors.Add($"Packaging template '{template}' is not supported.");
                    break;
            }
        }
    }
}
