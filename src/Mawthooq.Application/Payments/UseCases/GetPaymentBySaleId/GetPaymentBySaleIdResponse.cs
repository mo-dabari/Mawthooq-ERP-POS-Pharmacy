using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Payments.DTOs;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Payments.UseCases.GetPaymentBySaleId
{
    public record class GetPaymentBySaleIdResponse(
            int PaymentId,
            int SaleId,
            DateTime PaidAt,
            Currency Currency,
            IReadOnlyList<PaymentLineDto> PaymentLines,
            MoneyDto TotalAmount)
    {
    }
}
