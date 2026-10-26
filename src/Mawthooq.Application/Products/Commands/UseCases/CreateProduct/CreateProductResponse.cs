using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.LocalAgents.DTOs;
using Mawthooq.Application.Manufacturers.DTOs;
using Mawthooq.Application.Products.Commands.DTOs;

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
}
