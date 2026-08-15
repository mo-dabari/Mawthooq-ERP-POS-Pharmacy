using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.LocalAgents.DTOs;
using Mawthooq.Application.Manufacturers.DTOs;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Packaging;

namespace Mawthooq.Application.Products.Commands.UseCases.CreateProduct
{
    public record class CreateProductRequest(

        BasicInformation ProductInformationDto,

        PackagingInformation PackagingDto,

        List<UserUnitBarcodeDto?> UserBarcodes
        );
    public sealed record  UserUnitBarcodeDto(UnitOfMeasure Unit, BarcodeDto BarcodeDto);

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
            CategoryIdDto CategoryIdDto,
            ManufacturerIdDto ManufacturerIdDto,
            LocalAgentIdDto LocalAgentIdDto,
            PackagingTemplate Template,
            ImageDto Image,
            bool RequiresExpiryTracking = true,
            bool CanBeReturnedIfOpened = false,
            int NearExpiryThresholdInDays = 30);

}

