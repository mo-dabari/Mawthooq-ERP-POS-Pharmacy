using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Discounts;
using Mawthooq.Domain.Discounts.Enums;
using Mawthooq.Domain.Sales;
using Mawthooq.Domain.ValueObjects.Money;

namespace Mawthooq.Application.Discounts.UseCases.ApplyDiscountToSale
{
    public class ApplyDiscountToSaleUseCase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ISaleRepository _saleRepository;
        public ApplyDiscountToSaleUseCase(IDiscountRepository discountRepository, ISaleRepository saleRepository)
        {
            _discountRepository = discountRepository;
            _saleRepository = saleRepository;
        }

        public ApplyDiscountToSaleResponse Execute(ApplyDiscountToSaleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Discount? discount = _discountRepository.GetById(request.DiscountId);
            if (discount == null)
                throw new InvalidOperationException("Discount not found");

            if (!discount.IsActive)
                throw new InvalidOperationException("Discount is not active");

            if (discount.DiscountTarget != DiscountTarget.Invoice)
                throw new InvalidOperationException("This discount is not applicable to invoices.");

            Sale? sale = _saleRepository.GetById(request.SaleId);
            if (sale == null)
                throw new InvalidOperationException("Sale not found");

            if (discount.MinimumAmount is not null &&
                sale.TotalAmount.Amount < discount.MinimumAmount.Amount)
                throw new InvalidOperationException("Sale total does not meet the minimum amount for this discount.");

            Money discountAmount = discount.CalculateDiscountAmount(sale.TotalAmount);
            sale.ApplyInvoiceDiscount(discountAmount);
            _saleRepository.Update(sale);

            return new ApplyDiscountToSaleResponse(
                sale.SaleId, discount.DiscountId,
                 new MoneyDto(discountAmount.Amount, discountAmount.Currency));
        }
    }
}
