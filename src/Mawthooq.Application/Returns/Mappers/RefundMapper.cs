using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Returns.DTOs;
using Mawthooq.Domain.Returns.Entity;

namespace Mawthooq.Application.Returns.Mappers
{
    public static class RefundMapper
    {
        public static RefundDto ToDto(this Refund refund)
        {
            ArgumentNullException.ThrowIfNull(refund, nameof(refund));

            return new RefundDto(
                refund.RefundId,
                refund.Amount.ToDto(),
                refund.Status,
                refund.PaymentMethod);
        }

        public static Refund ToDomain(RefundDto refundDto)
        {
            ArgumentNullException.ThrowIfNull(refundDto, nameof(refundDto));

            return new Refund(
                refundDto.RefundId,
                refundDto.Amount.ToDomain(),
                refundDto.PaymentMethod);
        }
    }
}
