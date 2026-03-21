using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Payments.UseCases.CreatePayment
{
    public record class CreatePaymentResponse(int PaymentId, int SaleId, int BranchId, MoneyDto Amount, DateTime PaidAt)
    {

    }
}
