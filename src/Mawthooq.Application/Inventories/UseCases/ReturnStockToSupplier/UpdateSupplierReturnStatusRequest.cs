using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SupplierReturns.Enums;

namespace Mawthooq.Application.Inventories.UseCases.ReturnStockToSupplier
{
    public record UpdateSupplierReturnStatusRequest(
        int SupplierReturnId,
        ReturnStatus NewStatus
    )
    {

    }
}
