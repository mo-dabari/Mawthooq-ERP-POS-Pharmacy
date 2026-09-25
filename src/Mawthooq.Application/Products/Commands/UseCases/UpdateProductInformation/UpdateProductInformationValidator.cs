using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;

namespace Mawthooq.Application.Products.Commands.UseCases.UpdateProductInformation
{
    public static class UpdateProductInformationValidator
    {
        public static void Validate(UpdateProductInformationRequest request)
        {
            //     ProductIdDto ProductIdDto,
            // string TradeName,
            // CategoryIdDto CategoryIdDto,
            // ImageDto Image,
            // bool CanBeReturnedIfOpened,
            // int NearExpiryThresholdInDays,
            // byte[] RowVersion);

            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(request.ProductIdDto, nameof(request.ProductIdDto), errors))
                GuidValidator.ValidateAndCollect(request.ProductIdDto.Value, nameof(request.ProductIdDto.Value), errors);

            StringValidator.ValidateAndCollect(request.TradeName, nameof(request.TradeName), 50, errors);

            if (RequiredValidator.ValidateAndCollect(request.CategoryIdDto, nameof(request.CategoryIdDto), errors))
                GuidValidator.ValidateAndCollect(request.CategoryIdDto.Value, nameof(request.CategoryIdDto.Value), errors);

            if (RequiredValidator.ValidateAndCollect(
            request.Image,
            nameof(request.Image),
            errors))
            {
                RequiredValidator.ValidateAndCollect(
                request.Image.StorageKey,
                nameof(request.Image.StorageKey),
                errors);

                RequiredValidator.ValidateAndCollect(
                request.Image.FileName,
                nameof(request.Image.FileName),
                errors);

                RequiredValidator.ValidateAndCollect(
                request.Image.MimeType,
                nameof(request.Image.MimeType),
                errors);
            }

            if (request.RequiresExpiryTracking)
                {
                    RangeValidator.ValidateRange(
                        request.NearExpiryThresholdInDays,
                         30,
                         90,
                        nameof(request.NearExpiryThresholdInDays),
                         errors);
                }

            RowVersionValidator.ValidateAndCollect(request.RowVersion, errors);

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
