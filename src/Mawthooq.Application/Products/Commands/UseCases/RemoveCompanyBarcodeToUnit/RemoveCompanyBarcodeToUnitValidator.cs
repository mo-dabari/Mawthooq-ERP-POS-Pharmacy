using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;

namespace Mawthooq.Application.Products.Commands.UseCases.RemoveCompanyBarcodeToUnit
{
    public static class RemoveCompanyBarcodeToUnitValidator
    {
        public static void Validate(RemoveCompanyBarcodeToUnitRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductId, nameof(request.ProductId), errors))
                GuidValidator.ValidateAndCollect(request.ProductId.Value, nameof(request.ProductId), errors);

            if (RequiredValidator.ValidateAndCollect(request.PackagingVersionId, nameof(request.PackagingVersionId), errors))
                GuidValidator.ValidateAndCollect(request.PackagingVersionId.Value, nameof(request.PackagingVersionId), errors);

            EnumValidator.ValidateAndCollect(request.Unit, nameof(request.Unit), errors);

            if (RequiredValidator.ValidateAndCollect(request.Barcode, nameof(request.Barcode), errors))
            {
                RequiredValidator.ValidateAndCollect(
                            request.Barcode.Value,
                            nameof(request.Barcode.Value),
                            errors);

                EnumValidator.ValidateAndCollect(
                    request.Barcode.Type,
                    nameof(request.Barcode.Type),
                    errors);
            }

            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
