using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProducts
{
    public sealed record GetProductsItemDto(
        Guid ProductId,
        string InternalBarcode,
        string Sku,
        string TradeName,
        string CategoryName,
        string ManufacturerName,
        string LocalAgentName,
        Status Status);
}
