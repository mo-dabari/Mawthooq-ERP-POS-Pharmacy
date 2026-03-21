using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Payments.DTOs;
using Mawthooq.Domain.Payments.Entity;

namespace Mawthooq.Application.Payments.Mappers
{
    public static class PaymentLineMapper
    {
        public static PaymentLineDto ToDto(this PaymentLine paymentLine)
        {
            ArgumentNullException.ThrowIfNull(paymentLine, nameof(paymentLine));

            return new PaymentLineDto(paymentLine.Amount.ToDto(), paymentLine.PaymentMethod);
        }

        public static PaymentLine ToDomain(this PaymentLineDto paymentLineDto)
        {
            ArgumentNullException.ThrowIfNull(paymentLineDto, nameof(paymentLineDto));

            return new PaymentLine(paymentLineDto.Amount.ToDomain(), paymentLineDto.PaymentMethod);
        }
    }
}
