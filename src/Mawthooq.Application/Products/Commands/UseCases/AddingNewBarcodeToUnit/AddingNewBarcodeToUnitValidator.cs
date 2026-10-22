using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.UseCases.AddingNewBarcodeToUnit
{
    public static class AddingNewBarcodeToUnitValidator
    {
        public static void Validate(AddingNewBarcodeToUnitRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductIdDto, nameof(request.ProductIdDto), errors))
                GuidValidator.ValidateAndCollect(request.ProductIdDto.Value, nameof(request.ProductIdDto), errors);

            if (RequiredValidator.ValidateAndCollect(request.PackagingVersionIdDto, nameof(request.PackagingVersionIdDto), errors))
                GuidValidator.ValidateAndCollect(request.PackagingVersionIdDto.Value, nameof(request.PackagingVersionIdDto), errors);

            EnumValidator.ValidateAndCollect(request.UnitName, nameof(request.UnitName), errors);

            if (RequiredValidator.ValidateAndCollect(request.BarcodeDto, nameof(request.BarcodeDto), errors))
            {
                StringValidator.ValidateAndCollect(request.BarcodeDto.Value, nameof(request.BarcodeDto.Value), errors);
                EnumValidator.ValidateAndCollect(request.BarcodeDto.Type, nameof(request.BarcodeDto.Type), errors);
            }

            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
