using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Discounts;

namespace Mawthooq.Application.Discounts.UseCases.CreateDiscount
{
    public class CreateDiscountUseCase
    {
        private readonly IDiscountRepository _discountRepository;
        public CreateDiscountUseCase(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public CreateDiscountResponse Execute(CreateDiscountRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Discount discount = Discount.Create(request.Name, request.ProductId, request.DiscountType, request.DiscountTarget, request.Value, request.IsActive);
            Discount? savedDiscount = _discountRepository.Add(discount);

            return new CreateDiscountResponse(
                savedDiscount.DiscountId,
                savedDiscount.Name,
                savedDiscount.ProductId,
                savedDiscount.DiscountType.ToString(),
                savedDiscount.DiscountTarget.ToString(),
                savedDiscount.Value,
                savedDiscount.MinimumAmount is null ? null : new MoneyDto(savedDiscount.MinimumAmount.Amount, savedDiscount.MinimumAmount.Currency),
                savedDiscount.IsActive);
        }
    }
}
