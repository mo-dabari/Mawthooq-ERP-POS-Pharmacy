using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;

namespace Mawthooq.Application.Products.Commands.UseCases.ChangeProductCategory
{
    public static class ChangeProductCategoryValidator
    {
        public static void Validate(ChangeProductCategoryRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductIdDto, nameof(request.ProductIdDto), errors))
                GuidValidator.ValidateAndCollect(request.ProductIdDto.Value, nameof(request.ProductIdDto), errors);

            if (RequiredValidator.ValidateAndCollect(request.NewCategoryId, nameof(request.NewCategoryId), errors))
                GuidValidator.ValidateAndCollect(request.NewCategoryId.Value, nameof(request.NewCategoryId), errors);

            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
