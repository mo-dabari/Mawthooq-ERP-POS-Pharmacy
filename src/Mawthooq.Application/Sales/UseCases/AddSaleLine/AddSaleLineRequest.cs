using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Sales.UseCases.AddSaleLine
{
    public record class AddSaleLineRequest(int SaleId, int ProductId, int Quantity, MoneyDto SellingPrice, MoneyDto PurchaseCost)
    {

    }
}
