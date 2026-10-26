using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.ActivateProduct
{
    public record class ActivateProductResponse(
        ProductIdDto ProductId,
        string TradeName,
        Status Status,
        byte[] RowVersion);
}
