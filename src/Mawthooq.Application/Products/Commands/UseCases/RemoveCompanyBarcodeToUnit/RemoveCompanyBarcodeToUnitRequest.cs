using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.RemoveCompanyBarcodeToUnit
{
    public record class RemoveCompanyBarcodeToUnitRequest(
        ProductIdDto ProductIdDto,
        PackagingVersionIdDto PackagingVersionIdDto,
        UnitOfMeasure UnitName,
        BarcodeDto BarcodeDto,
        byte[] RowVersion);

}
