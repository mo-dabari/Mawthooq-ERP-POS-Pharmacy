using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Application.Products.Commands.Validation;

namespace Mawthooq.Application.Products.Commands.UseCases.AddPackagingVersion
{
    public static class AddPackagingVersionValidator
    {
        public static void Validate(AddPackagingVersionRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductIdDto, nameof(request.ProductIdDto), errors))
                GuidValidator.ValidateAndCollect(request.ProductIdDto.Value, nameof(request.ProductIdDto), errors);

            EnumValidator.ValidateAndCollect(request.Template, nameof(request.Template), errors);

            if (RequiredValidator.ValidateAndCollect(request.Packaging, nameof(request.Packaging), errors))
                PackagingInformationValidator.ValidateAndCollect(request.Template, request.Packaging, errors);

            RowVersionValidator.ValidateAndCollect(request.ExpectedRowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
