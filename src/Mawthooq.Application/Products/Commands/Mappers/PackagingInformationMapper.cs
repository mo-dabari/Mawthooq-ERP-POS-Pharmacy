using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Packaging;

namespace Mawthooq.Application.Products.Commands.Mappers
{
    public static class PackagingInformationMapper
    {
        public static ProductPackaging ToDomain(this PackagingInformation dto, PackagingTemplate template)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            return ProductPackagingFactory.Create(
                template,
                dto.TotalUnitsPerBox,
                dto.DirectUnit,
                dto.StripsPerBox,
                dto.UnitsPerStrip,
                dto.InnerBoxesPerOuterBox,
                dto.PiecesPerInnerBox,
                dto.SmallUnit);
        }
    }
}
