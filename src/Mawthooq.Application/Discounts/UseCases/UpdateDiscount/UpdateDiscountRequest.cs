using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Discounts.UseCases.UpdateDiscount
{
    public record class UpdateDiscountRequest(int DiscountId,bool IsActive)
    {

    }
}
