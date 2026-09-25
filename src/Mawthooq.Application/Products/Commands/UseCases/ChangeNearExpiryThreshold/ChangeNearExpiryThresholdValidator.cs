using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;

namespace Mawthooq.Application.Products.Commands.UseCases.ChangeNearExpiryThreshold
{
    public static class ChangeNearExpiryThresholdValidator
    {
        public static void Validate(ChangeNearExpiryThresholdRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductIdDto, nameof(request.ProductIdDto), errors))
                GuidValidator.ValidateAndCollect(request.ProductIdDto.Value, nameof(request.ProductIdDto), errors);

            NumberValidator.ValidateNonNegative(request.NewNearExpiryThresholdInDays, nameof(request.NewNearExpiryThresholdInDays), errors);

            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
