using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Discounts;

namespace Mawthooq.Application.Discounts.UseCases.UpdateDiscount
{
    public class UpdateDiscountUseCase
    {
        private readonly IDiscountRepository _discountRepository;
        public UpdateDiscountUseCase(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public UpdateDiscountResponse Execute(UpdateDiscountRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Discount? discount = _discountRepository.GetById(request.DiscountId);
            if (discount == null)
                throw new InvalidOperationException("Discount not found");

            if (request.IsActive)
                discount.Activate();
            else
                discount.Deactivate();

            _discountRepository.Update(discount);

            return new UpdateDiscountResponse(
                discount.DiscountId,
                discount.Name,
                discount.ProductId,
                discount.DiscountType.ToString(),
                discount.DiscountTarget.ToString(),
                discount.Value,
                discount.MinimumAmount is null ? null : new MoneyDto(discount.MinimumAmount.Amount, discount.MinimumAmount.Currency),
                discount.IsActive
            );
        }

    }
}
