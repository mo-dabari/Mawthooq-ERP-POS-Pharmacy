using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Application.Products.Commands.Validation;

namespace Mawthooq.Application.Products.Commands.UseCases.CreateProduct
{
    public static class CreateProductValidator
    {
        public static void Validate(CreateProductRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(
                request.ProductInformationDto,
                nameof(request.ProductInformationDto),
                errors))
            {
                StringValidator.ValidateAndCollect(
                    request.ProductInformationDto.TradeName,
                    nameof(request.ProductInformationDto.TradeName),
                    50,
                    errors);

                if (RequiredValidator.ValidateAndCollect(
                    request.ProductInformationDto.Sku,
                    nameof(request.ProductInformationDto.Sku),
                    errors))
                {
                    RequiredValidator.ValidateAndCollect(
                        request.ProductInformationDto.Sku.Value,
                        nameof(request.ProductInformationDto.Sku),
                        errors);
                }

                if (RequiredValidator.ValidateAndCollect(
                    request.ProductInformationDto.CategoryIdDto,
                    nameof(request.ProductInformationDto.CategoryIdDto),
                    errors))
                {
                    GuidValidator.ValidateAndCollect(
                        request.ProductInformationDto.CategoryIdDto.Value,
                        nameof(request.ProductInformationDto.CategoryIdDto),
                        errors);
                }

                if (RequiredValidator.ValidateAndCollect(
                    request.ProductInformationDto.ManufacturerIdDto,
                    nameof(request.ProductInformationDto.ManufacturerIdDto),
                    errors))
                {
                    GuidValidator.ValidateAndCollect(
                        request.ProductInformationDto.ManufacturerIdDto.Value,
                        nameof(request.ProductInformationDto.ManufacturerIdDto),
                        errors);
                }

                if (RequiredValidator.ValidateAndCollect(
                    request.ProductInformationDto.LocalAgentIdDto,
                    nameof(request.ProductInformationDto.LocalAgentIdDto),
                    errors))
                {
                    GuidValidator.ValidateAndCollect(
                        request.ProductInformationDto.LocalAgentIdDto.Value,
                        nameof(request.ProductInformationDto.LocalAgentIdDto),
                        errors);
                }

                EnumValidator.ValidateAndCollect(
                    request.ProductInformationDto.Template,
                    nameof(request.ProductInformationDto.Template),
                    errors);

                if (RequiredValidator.ValidateAndCollect(
                request.ProductInformationDto.Image,
                nameof(request.ProductInformationDto.Image),
                errors))
                {
                    RequiredValidator.ValidateAndCollect(
                    request.ProductInformationDto.Image.StorageKey,
                    nameof(request.ProductInformationDto.Image.StorageKey),
                    errors);

                    RequiredValidator.ValidateAndCollect(
                    request.ProductInformationDto.Image.FileName,
                    nameof(request.ProductInformationDto.Image.FileName),
                    errors);

                    RequiredValidator.ValidateAndCollect(
                    request.ProductInformationDto.Image.MimeType,
                    nameof(request.ProductInformationDto.Image.MimeType),
                    errors);
                }

                if (request.ProductInformationDto.RequiresExpiryTracking)
                {
                    RangeValidator.ValidateRange(
                        request.ProductInformationDto.NearExpiryThresholdInDays,
                         30,
                         90,
                        nameof(request.ProductInformationDto.NearExpiryThresholdInDays),
                         errors);
                }

                PackagingInformationValidator.ValidateAndCollect(
                    request.ProductInformationDto.Template,
                    request.PackagingDto,
                    errors);

                if (request.UserBarcodes is not null)
                {
                    foreach (var userBarcode in request.UserBarcodes)
                    {
                        EnumValidator.ValidateAndCollect(
                            userBarcode.Unit,
                            nameof(userBarcode.Unit),
                            errors);

                        RequiredValidator.ValidateAndCollect(
                            userBarcode.BarcodeDto,
                            nameof(userBarcode.BarcodeDto),
                            errors);
                    }

                    var duplicateBarcodes = request.UserBarcodes?
                    .GroupBy(b => b.BarcodeDto?.Value)
                    .Where(g => g.Key is not null && g.Count() > 1)
                    .Select(g => g.Key!);

                    if (duplicateBarcodes?.Any() == true)
                        errors.Add($"Duplicate barcode value(s) in request: {string.Join(", ", duplicateBarcodes)}");
                }
            }

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
