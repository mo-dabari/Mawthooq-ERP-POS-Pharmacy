using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Application.Sales.UseCases.ConfirmSale
{
    public record class ConfirmSaleResponse (int SaleId, int BranchId, MoneyDto TotalAmount)
    {

    }
}
