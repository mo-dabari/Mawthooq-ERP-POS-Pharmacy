using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.LocalAgents.DTOs;
using Mawthooq.Application.Manufacturers.DTOs;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.CreateProduct
{
    public record class CreateProductRequest(

        BasicInformation ProductInformation,

        PackagingInformation Packaging,

        List<UserUnitBarcodeDto>? UserBarcodes
        );
    public sealed record UserUnitBarcodeDto(UnitOfMeasure Unit, BarcodeDto Barcode);

    public record class PackagingInformation(
        int? TotalUnitsPerBox,
        UnitOfMeasure? DirectUnit,
        int? StripsPerBox,
        int? UnitsPerStrip,
        int? InnerBoxesPerOuterBox,
        int? PiecesPerInnerBox,
        UnitOfMeasure? SmallUnit
    );

    public sealed record BasicInformation(
            string TradeName,
            SkuDto Sku,
            CategoryIdDto CategoryId,
            ManufacturerIdDto ManufacturerId,
            LocalAgentIdDto LocalAgentId,
            PackagingTemplate Template,
            ImageDto Image,
            bool RequiresExpiryTracking,
            bool CanBeReturnedIfOpened,
            int NearExpiryThresholdInDays);

}

