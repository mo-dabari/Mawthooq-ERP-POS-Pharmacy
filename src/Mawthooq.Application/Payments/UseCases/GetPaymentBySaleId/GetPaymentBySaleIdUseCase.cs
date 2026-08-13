using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Payments.DTOs;
using Mawthooq.Domain.Payments;

namespace Mawthooq.Application.Payments.UseCases.GetPaymentBySaleId
{
    public class GetPaymentBySaleIdUseCase
    {
        private readonly IPaymentRepository _paymentRepository;
        public GetPaymentBySaleIdUseCase(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public GetPaymentBySaleIdResponse Execute(GetPaymentBySaleIdRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Payment? payment = _paymentRepository.GetBySaleId(request.SaleId);
            if (payment == null)
                throw new InvalidOperationException("Payment not found for the given Sale ID.");

            return new GetPaymentBySaleIdResponse(
                payment.PaymentId,
                payment.SaleId,
                payment.PaidAt,
                payment.Currency,
                payment.PaymentLines.Select(line => new PaymentLineDto(line.Amount.ToDto(), line.PaymentMethod)).ToImmutableList(),
                payment.TotalAmount.ToDto()
            );
        }
    }
}
