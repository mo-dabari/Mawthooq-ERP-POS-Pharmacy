using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;

namespace Mawthooq.Application.Products.Commands.UseCases.ActivateProduct
{
    public record class ActivateProductRequest(
        ProductIdDto ProductIdDto,
        byte[] RowVersion);
}
