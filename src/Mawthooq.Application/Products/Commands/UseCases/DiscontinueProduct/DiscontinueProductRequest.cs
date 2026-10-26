using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.DiscontinueProduct
{
    public record class DiscontinueProductRequest(
        ProductIdDto ProductId,
        byte[] RowVersion);
}
