using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.DTOs;
using Mawthooq.Application.SupplierReturns.DTOs;
using Mawthooq.Domain.SupplierReturns.Enums;

namespace Mawthooq.Application.SupplierReturns.UseCases.GetSupplierReturnsBySupplierId
{
    public record class GetSupplierReturnsBySupplierIdResponse(
         int SupplierId,
         IReadOnlyList<SupplierReturnDto> SupplierReturns)
    {

    }
}
