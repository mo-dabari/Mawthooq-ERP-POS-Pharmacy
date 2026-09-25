using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Application.Products.Commands.DTOs;

namespace Mawthooq.Application.Products.Commands.UseCases.ActivatePackagingVersion
{
    public static class ActivatePackagingVersionValidator
    {
        public static void Validate(ActivatePackagingVersionRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductIdDto, nameof(request.ProductIdDto), errors))
                GuidValidator.ValidateAndCollect(request.ProductIdDto.Value, nameof(request.ProductIdDto), errors);


            if (RequiredValidator.ValidateAndCollect(request.PackagingVersionId, nameof(request.PackagingVersionId), errors))
                GuidValidator.ValidateAndCollect(request.PackagingVersionId.Value, nameof(request.PackagingVersionId), errors);


            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
