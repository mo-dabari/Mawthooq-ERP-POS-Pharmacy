using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Suppliers.DTOs
{
    public record class PaymentTermsDto(
        short NetDays,
        short DiscountDays,
        decimal DiscountPercentage,
        decimal PenaltyPercentage
    )
    {

    }
}
