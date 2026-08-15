using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.RemoveCompanyBarcodeToUnit
{
    public record class RemoveCompanyBarcodeToUnitResponse(
        ProductIdDto ProductIdDto,
        UnitOfMeasure UnitName,
        IReadOnlyList<BarcodeDto> Barcodes,
        byte[] NewRowVersion);

}
