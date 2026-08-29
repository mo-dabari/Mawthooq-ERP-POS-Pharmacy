using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.DiscontinueProduct
{
    public record class DiscontinueProductResponse(
        ProductIdDto ProductId,
        string Name,
        Status NewStatus,
        byte[] RowVersion);
}
