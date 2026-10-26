using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.AddingNewBarcodeToUnit
{
    public record class AddingNewBarcodeToUnitResponse(
        ProductIdDto ProductId,
        PackagingVersionIdDto PackagingVersionId,
        UnitOfMeasure Unit,
        IReadOnlyList<BarcodeDto> Barcodes,
        byte[] RowVersion);

}
