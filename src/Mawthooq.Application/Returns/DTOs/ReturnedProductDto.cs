using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Returns.Enums;

namespace Mawthooq.Application.Returns.DTOs
{
    public record class ReturnedProductDto(
        int ProductId,
        byte Quantity,
        ReturnReason Reason,
        ProductReturnStatus Status,
        DateOnly ExpiryDate)
    {
    }
}
