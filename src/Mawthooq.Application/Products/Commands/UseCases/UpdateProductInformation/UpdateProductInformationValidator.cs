using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.UpdateProductInformation
{
    public static class UpdateProductInformationValidator
    {
        public static void Validate(UpdateProductInformationRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductId, nameof(request.ProductId), errors))
                GuidValidator.ValidateAndCollect(
                    request.ProductId.Value,
                    nameof(request.ProductId.Value),
                    errors);

            StringValidator.ValidateAndCollect(
                request.TradeName,
                nameof(request.TradeName),
                Product.MaxTradeNameLength,
                errors);

            if (RequiredValidator.ValidateAndCollect(request.CategoryId, nameof(request.CategoryId), errors))
                GuidValidator.ValidateAndCollect(
                    request.CategoryId.Value,
                    nameof(request.CategoryId.Value),
                    errors);

            ImageValidator.Validate(request.Image, nameof(request.Image), errors);

            NullableValidator.ValidateRequired(
                request.CanBeReturnedIfOpened,
                nameof(request.CanBeReturnedIfOpened),
                errors);

            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
