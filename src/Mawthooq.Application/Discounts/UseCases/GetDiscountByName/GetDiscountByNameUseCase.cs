using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Domain.Discounts;

namespace Mawthooq.Application.Discounts.UseCases.GetDiscountByName
{
    public class GetDiscountByNameUseCase
    {
        private readonly IDiscountRepository _discountRepository;
        public GetDiscountByNameUseCase(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public GetDiscountByNameResponse Execute(GetDiscountByNameRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Discount? discount = _discountRepository.GetByName(request.Name);
            if (discount is null)
                throw new InvalidOperationException($"Discount with name {request.Name} not found.");

            return new GetDiscountByNameResponse(
                discount.DiscountId,
                discount.Name,
                discount.ProductId,
                discount.DiscountType.ToString(),
                discount.DiscountTarget.ToString(),
                discount.Value,
                discount.MinimumAmount is null ? null : new MoneyDto(discount.MinimumAmount.Amount, discount.MinimumAmount.Currency),
                discount.IsActive);
        }
    }
}
