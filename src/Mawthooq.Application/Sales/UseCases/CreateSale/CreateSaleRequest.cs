using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Sales.DTOs;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Application.Sales.UseCases.CreateSale
{
    public record class CreateSaleRequest(
        int BranchId,
        IReadOnlyList<CreateSaleLineRequest> SaleLines,
        Currency Currency,
        int? CustomerId = null)
    {
    }
}
