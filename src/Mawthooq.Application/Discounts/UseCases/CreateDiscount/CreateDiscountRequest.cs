using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Discounts.Enums;

namespace Mawthooq.Application.Discounts.UseCases.CreateDiscount
{
    public record class CreateDiscountRequest(
        string Name,
        int? ProductId,
        DiscountType DiscountType,
        DiscountTarget DiscountTarget,
        decimal Value,
        MoneyDto? MinimumAmount,
        bool IsActive)
    {

    }
}
