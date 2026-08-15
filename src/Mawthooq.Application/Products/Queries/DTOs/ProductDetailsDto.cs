using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Queries.UseCases.GetProductDetails;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Queries.DTOs
{
    public record class ProductDetailsDto(
        Guid ProductId,
        string DisplayName,
        string SKU,
        string Category,
        string Manufacturer,
        string LocalAgent,
        bool CanBeReturnedIfOpened,
        string Status,
        string? ImageUrl,
        IEnumerable<PackagingVersionDto> Barcode);


    public record class PackagingVersionDto(
        Guid PackagingVersionId,
        int Version,
        string Status,
        IEnumerable<ProductUnitDetailsDto> ProductUnits
    );

    public record class ProductUnitDetailsDto(
        string UnitName,
        int ConversionRate,
        string RankName,
        IEnumerable<BarcodeDto> ProductUnits
    );
}
