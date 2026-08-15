using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductLookup
{
    public sealed record GetProductLookupItemDto(
         Guid ProductId,
         string DisplayName,
         string Sku,
         string InternalBarcode);
}
