using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;

namespace Mawthooq.Application.Products.Commands.UseCases.DeactivatePackagingVersion
{
    public static class DeactivatePackagingVersionValidator
    {
        public static void Validate(DeactivatePackagingVersionRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductIdDto, nameof(request.ProductIdDto), errors))
                GuidValidator.ValidateAndCollect(request.ProductIdDto.Value, nameof(request.ProductIdDto), errors);

            if (RequiredValidator.ValidateAndCollect(request.packagingVersionId, nameof(request.packagingVersionId), errors))
                GuidValidator.ValidateAndCollect(request.packagingVersionId.Value, nameof(request.packagingVersionId), errors);

            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
