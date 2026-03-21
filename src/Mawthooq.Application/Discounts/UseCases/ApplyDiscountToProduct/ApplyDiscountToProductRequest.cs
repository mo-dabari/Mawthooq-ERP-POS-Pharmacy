using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Discounts.UseCases.ApplyDiscountToProduct
{
    public record class ApplyDiscountToProductRequest(int SaleId, int ProductId, int DiscountId)
    {

    }
}
