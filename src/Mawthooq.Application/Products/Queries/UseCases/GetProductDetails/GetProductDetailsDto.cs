using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.LocalAgents.DTOs;
using Mawthooq.Application.Manufacturers.DTOs;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductDetails
{
    public record class GetProductDetailsDto(
    ProductIdDto ProductId,
    string TradeName,
    string SKU,
    CategoryIdDto CategoryId,
    string CategoryName,
    ManufacturerIdDto ManufacturerId,
    string ManufacturerName,
    LocalAgentIdDto LocalAgentId,
    string LocalAgentName,
    bool RequiresExpiryTracking,
    bool CanBeReturnedIfOpened,
    int NearExpiryThresholdInDays,
    Status Status,
    ImageDto Image,
    IReadOnlyList<PackagingVersionDto> PackagingVersions,
    byte[] RowVersion);

}
