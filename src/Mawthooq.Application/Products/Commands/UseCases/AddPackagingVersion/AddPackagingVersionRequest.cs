using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.AddPackagingVersion
{
    public sealed record AddPackagingVersionRequest(
        ProductIdDto ProductId,
        PackagingTemplate Template,
        PackagingInformation Packaging,
        byte[] RowVersion);
}
