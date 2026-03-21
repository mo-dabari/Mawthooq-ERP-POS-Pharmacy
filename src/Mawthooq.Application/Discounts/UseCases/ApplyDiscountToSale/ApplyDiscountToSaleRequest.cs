using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Discounts.UseCases.ApplyDiscountToSale
{
    public record class ApplyDiscountToSaleRequest(int SaleId, int DiscountId)
    {

    }
}
