using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Payments.DTOs;
using Mawthooq.Domain.Payments;
using Mawthooq.Domain.Payments.Entity;
using Mawthooq.Domain.Sales;
using Mawthooq.Domain.Sales.Enums;

namespace Mawthooq.Application.Payments.UseCases.CreatePayment
{
    public class CreatePaymentUseCase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISaleRepository _saleRepository;
        public CreatePaymentUseCase(IPaymentRepository paymentRepository, ISaleRepository saleRepository)
        {
            _paymentRepository = paymentRepository;
            _saleRepository = saleRepository;
        }

        public CreatePaymentResponse Execute(CreatePaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Sale? sale = _saleRepository.GetById(request.SaleId);
            if (sale == null)
                throw new InvalidOperationException("Sale not found.");

            if (sale.BranchId != request.BranchId)
                throw new InvalidOperationException("Sale does not belong to the specified branch.");

            if (sale.Status != SaleStatus.Confirmed)
                throw new InvalidOperationException("Cannot process payment for a non-confirmed sale.");

            Payment payment = Payment.Create(request.SaleId, sale.Currency);

            foreach (PaymentLineDto line in request.PaymentLines)
            {
                if (line.Amount.Currency != sale.Currency)
                    throw new InvalidOperationException("Currency mismatch.");

                payment.AddPaymentLine(line.Amount.ToDomain(), line.PaymentMethod);
            }

            Payment? savedPayment = _paymentRepository.Add(payment);

            if (savedPayment is null)
                throw new InvalidOperationException("Failed to create payment.");

            return new CreatePaymentResponse(savedPayment.PaymentId, savedPayment.SaleId, sale.BranchId, savedPayment.TotalAmount.ToDto(), savedPayment.PaidAt);
        }
    }
}
