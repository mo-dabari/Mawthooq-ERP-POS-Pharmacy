using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Payments.Enums;
using Mawthooq.Domain.Returns.Enums;

namespace Mawthooq.Application.Returns.DTOs
{
    public record class RefundDto (int RefundId, MoneyDto Amount, RefundStatus Status, PaymentMethod PaymentMethod)
    {
    }
}
