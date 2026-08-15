using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductDetails
{
    public record class GetProductDetailsDto(
    Guid ProductId,
    string DisplayName,
    string SKU,
    string Category,
    string Manufacturer,
    string LocalAgent,
    bool CanBeReturnedIfOpened,
    Status Status,
    string? ImageUrl,
    IReadOnlyList<PackagingVersionDto> PackagingVersions);

    public record class PackagingVersionDto(
        Guid PackagingVersionId,
        int Version,
        string Status,
        IReadOnlyList<ProductUnitDetailsDto> ProductUnits
    );

    public record class ProductUnitDetailsDto(
        string UnitName,
        int ConversionRate,
        string RankName,
        IReadOnlyList<BarcodeDto> Barcodes
    );

    public record class BarcodeDto(
        string Value,
        string TypeName
    );

}
