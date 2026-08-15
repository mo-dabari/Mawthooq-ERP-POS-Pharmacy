using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;

namespace Mawthooq.Application.Products.Queries.UseCases.GetProductDetails
{
    public sealed record GetProductDetailsQuery(ProductIdDto ProductId);
}
