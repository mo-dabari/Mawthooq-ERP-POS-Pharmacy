using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.SupplierReturns.DTOs;
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
