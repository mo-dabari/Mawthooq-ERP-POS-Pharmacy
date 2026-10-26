using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Products.Commands.DTOs
{
    public record class PackagingVersionDto
    (
        PackagingVersionIdDto PackagingVersionId,
        int Version,
        Status Status,
        IReadOnlyList<ProductUnitDetailsDto> Units
    );
}
