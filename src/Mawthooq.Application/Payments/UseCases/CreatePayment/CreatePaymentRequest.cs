using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Payments.DTOs;

namespace Mawthooq.Application.Payments.UseCases.CreatePayment
{
    public record class CreatePaymentRequest (int SaleId, int BranchId, IReadOnlyList<PaymentLineDto> PaymentLines)
    {

    }
}
