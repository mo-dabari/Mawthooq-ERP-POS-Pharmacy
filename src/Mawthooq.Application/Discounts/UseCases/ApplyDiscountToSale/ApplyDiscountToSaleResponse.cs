using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Discounts.UseCases.ApplyDiscountToSale
{
    public record class ApplyDiscountToSaleResponse (int SaleId, int DiscountId, MoneyDto DiscountAmount)
    {

    }
}
