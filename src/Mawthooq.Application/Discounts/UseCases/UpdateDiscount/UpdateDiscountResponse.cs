using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Discounts.UseCases.UpdateDiscount
{
    public record class UpdateDiscountResponse(
        int DiscountId,
        string Name,
        int? ProductId,
        string DiscountType,
        string DiscountTarget,
        decimal Value,
        MoneyDto? MinimumAmount,
        bool IsActive)
    {

    }
}
