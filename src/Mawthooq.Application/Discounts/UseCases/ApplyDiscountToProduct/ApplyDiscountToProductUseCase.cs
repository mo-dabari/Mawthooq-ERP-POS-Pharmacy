using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Discounts;
using Mawthooq.Domain.Discounts.Enums;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Sales;
using Mawthooq.Domain.Sales.Entity;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Application.Discounts.UseCases.ApplyDiscountToProduct
{
    public class ApplyDiscountToProductUseCase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ISaleRepository _saleRepository;
        public ApplyDiscountToProductUseCase(IDiscountRepository discountRepository, ISaleRepository saleRepository)
        {
            _discountRepository = discountRepository;
            _saleRepository = saleRepository;
        }

        public ApplyDiscountToProductResponse Execute(ApplyDiscountToProductRequest request)
        {
            var discount = _discountRepository.GetById(request.DiscountId);
            if (discount == null)
                throw new InvalidOperationException("Discount not found");

            Sale? sale = _saleRepository.GetById(request.SaleId);
            if (sale == null)
                throw new InvalidOperationException("Sale not found");

            if (!discount.IsActive)
                throw new InvalidOperationException("Discount is not active");

            SaleLine? saleLine = sale.Lines.FirstOrDefault(l => l.ProductId == request.ProductId);
            if (saleLine is null)
                throw new InvalidOperationException("Product not found in sale.");

            if (discount.MinimumAmount is not null &&
                saleLine.TotalAmount.Amount < discount.MinimumAmount.Amount)
                throw new InvalidOperationException("Product total does not meet the minimum amount.");

            if (discount.DiscountTarget != DiscountTarget.Product)
                throw new InvalidOperationException("This discount is not applicable to line items.");

            Money discountAmount = discount.CalculateDiscountAmount(saleLine.TotalAmount);
            saleLine.ApplyDiscount(discountAmount);

            _saleRepository.Update(sale);

            return new ApplyDiscountToProductResponse(request.ProductId, request.DiscountId, new MoneyDto(discountAmount.Amount, discountAmount.Currency));
        }
    }
}
