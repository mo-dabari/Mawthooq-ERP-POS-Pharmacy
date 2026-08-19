using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.LocalAgents.DTOs;
using Mawthooq.Application.Manufacturers.DTOs;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Categories.Enums;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.CreateProduct
{
    public record class CreateProductResponse(
        ProductIdDto ProductId,
        string TradeName,
        SkuDto Sku,
        CategoryIdDto CategoryId,
        ManufacturerIdDto ManufacturerId,
        LocalAgentIdDto LocalAgentId,
        bool RequiresExpiryTracking,
        bool CanBeReturnedIfOpened,
        int NearExpiryThresholdInDays,
        ImageDto Image,

        IReadOnlyList<PackagingVersionDto> PackagingVersions,
        byte[] RowVersion);

    public record PackagingVersionDto(
        PackagingVersionIdDto PackagingVersionId,
        int Version,
        Status Status,
        IReadOnlyList<ProductUnitDetailsDto> Units);

    public record class ProductUnitDetailsDto(
    UnitOfMeasure Unit,
    int ConversionRate,
    UnitRank Rank,
    IReadOnlyList<BarcodeDto> Barcodes);
}
