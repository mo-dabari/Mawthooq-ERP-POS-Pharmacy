using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Sales.DTOs;
using Mawthooq.Domain.Sales.Enums;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Application.Sales.UseCases.CreateSale
{
    public record class CreateSaleResponse(
        int SaleId,
        int BranchId,
        Currency Currency,
        int? CustomerId,
        DateTime SaleDate,
        SaleStatus Status,
        IReadOnlyList<SaleLineDto> Lines,
        MoneyDto FinalAmount,
        MoneyDto TotalAmount,
        MoneyDto TotalPurchaseCost,
        MoneyDto TotalGrossProfit
        )
    {

    }
}
