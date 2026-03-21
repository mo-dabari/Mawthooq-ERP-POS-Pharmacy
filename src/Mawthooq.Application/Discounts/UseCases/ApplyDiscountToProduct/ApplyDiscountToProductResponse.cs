using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Discounts.UseCases.ApplyDiscountToProduct
{
    public record class ApplyDiscountToProductResponse(int ProductId, int DiscountId, MoneyDto DiscountAmount)
    {

    }
}
