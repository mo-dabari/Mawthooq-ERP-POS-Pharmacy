using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Inventories.UseCases.ReturnStockToSupplier.DTOs;
using Mawthooq.Domain.SupplierReturns.Enums;

namespace Mawthooq.Application.SupplierReturns.UseCases.UpdateSupplierReturnStatus
{
    public record class UpdateSupplierReturnStatusResponse(
        SupplierReturnDto UpdatedSupplierReturn,
        MoneyDto? TotalReturnAmount
    )
    {
    }
}
