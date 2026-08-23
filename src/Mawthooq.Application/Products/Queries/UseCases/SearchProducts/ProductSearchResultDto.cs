using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Queries.UseCases.SearchProducts
{
    public sealed record ProductSearchResultDto(
        Guid ProductId,
        string TradeName,
        string Sku,
        string MatchedBarcode,
        string CategoryName,
        Status Status);
}
