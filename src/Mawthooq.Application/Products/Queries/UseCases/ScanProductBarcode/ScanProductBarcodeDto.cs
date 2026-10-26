using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Queries.UseCases.ScanProductBarcode
{
    public sealed record ScanProductBarcodeDto(
        Guid ProductId,
        string TradeName,
        string Sku,
        Guid PackagingVersionId,
        string Unit,
        int ConversionRate,
        string MatchedBarcode,
        Status Status);
}
