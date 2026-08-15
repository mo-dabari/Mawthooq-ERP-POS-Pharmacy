using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.ActivatePackagingVersion
{
    public record class ActivatePackagingVersionResponse(
        ProductIdDto ProductIdDto,
        PackagingVersionIdDto PackagingVersionId,
        Status Status,
        byte[] RowVersion);
}
