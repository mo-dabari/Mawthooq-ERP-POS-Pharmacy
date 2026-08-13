using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Sales.DTOs;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Application.Sales.UseCases.GetSaleById
{
    public record class GetSaleByIdResponse (int SaleId, int BranchId, IReadOnlyList<SaleLineDto> SaleLines, Currency Currency)
    {

    }
}
