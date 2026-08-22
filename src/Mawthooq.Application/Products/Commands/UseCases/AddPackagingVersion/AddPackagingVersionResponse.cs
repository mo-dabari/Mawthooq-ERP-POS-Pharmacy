using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;

namespace Mawthooq.Application.Products.Commands.UseCases.AddPackagingVersion
{
    public sealed record AddPackagingVersionResponse(
        ProductIdDto ProductId,
        PackagingVersionDto PackagingVersion,
        byte[] RowVersion);
}
