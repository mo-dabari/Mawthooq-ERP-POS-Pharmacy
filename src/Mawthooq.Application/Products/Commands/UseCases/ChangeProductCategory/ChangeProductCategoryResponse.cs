using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.DTOs;
using Mawthooq.Application.Products.Commands.DTOs;

namespace Mawthooq.Application.Products.Commands.UseCases.ChangeProductCategory
{
    public record class ChangeProductCategoryResponse(
        ProductIdDto ProductId,
        CategoryIdDto CategoryId,
        byte[] RowVersion);
}
