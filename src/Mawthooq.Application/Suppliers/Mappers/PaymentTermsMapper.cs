using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Suppliers.DTOs;
using Mawthooq.Domain.Suppliers.ValueObjects;

namespace Mawthooq.Application.Suppliers.Mappers
{
    public static class PaymentTermsMapper
    {
        public static PaymentTerms ToDomain(this PaymentTermsDto paymentTermsDto)
        {
            ArgumentNullException.ThrowIfNull(paymentTermsDto, nameof(paymentTermsDto));

            return new PaymentTerms(
                paymentTermsDto.NetDays,
                paymentTermsDto.DiscountDays,
                paymentTermsDto.DiscountPercentage,
                paymentTermsDto.PenaltyPercentage
                 );
        }

        public static PaymentTermsDto ToDto(this PaymentTerms paymentTermsDto)
        {
            ArgumentNullException.ThrowIfNull(paymentTermsDto, nameof(paymentTermsDto));

            return new PaymentTermsDto(
                paymentTermsDto.NetDays,
                paymentTermsDto.DiscountDays,
                paymentTermsDto.DiscountPercentage,
                paymentTermsDto.PenaltyPercentage
                 );
        }
    }
}
