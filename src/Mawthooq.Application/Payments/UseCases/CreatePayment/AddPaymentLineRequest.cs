using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Payments.Enums;

namespace Mawthooq.Application.Payments.UseCases.CreatePayment
{
    public record class AddPaymentLineRequest(MoneyDto Amount, PaymentMethod PaymentMethod)
    {

    }
}
