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

            if (RequiredValidator.ValidateAndCollect(request.ProductId, nameof(request.ProductId), errors))
                GuidValidator.ValidateAndCollect(request.ProductId.Value, nameof(request.ProductId), errors);

            if (RequiredValidator.ValidateAndCollect(request.PackagingVersionId, nameof(request.PackagingVersionId), errors))
                GuidValidator.ValidateAndCollect(request.PackagingVersionId.Value, nameof(request.PackagingVersionId), errors);

            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
