using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Sales.UseCases.CreateSale
{
    public record class CreateSaleLineRequest(

        int ProductId,
        int Quantity,
        MoneyDto SellingPrice,
        MoneyDto PurchaseCost)
    {
    }
}
