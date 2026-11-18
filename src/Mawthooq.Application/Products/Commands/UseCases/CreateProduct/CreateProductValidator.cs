using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;
using Mawthooq.Application.Products.Commands.Validation;
using Mawthooq.Domain.Products;

namespace Mawthooq.Application.Products.Commands.UseCases.CreateProduct
{
    public static class CreateProductValidator
    {
        public static void Validate(CreateProductRequest request)
        {
            List<string> errors = new();

            if (RequiredValidator.ValidateAndCollect(
                request.ProductInformation,
                nameof(request.ProductInformation),
                errors))
            {
                StringValidator.ValidateAndCollect(
                    request.ProductInformation.TradeName,
                    nameof(request.ProductInformation.TradeName),
                    Product.MaxTradeNameLength,
                    errors);

                if (RequiredValidator.ValidateAndCollect(
                    request.ProductInformation.Sku,
                    nameof(request.ProductInformation.Sku),
                    errors))
                {
                    RequiredValidator.ValidateAndCollect(
                        request.ProductInformation.Sku.Value,
                        nameof(request.ProductInformation.Sku),
                        errors);
                }

                if (RequiredValidator.ValidateAndCollect(
                    request.ProductInformation.CategoryId,
                    nameof(request.ProductInformation.CategoryId),
                    errors))
                {
                    GuidValidator.ValidateAndCollect(
                        request.ProductInformation.CategoryId.Value,
                        nameof(request.ProductInformation.CategoryId),
                        errors);
                }

                if (RequiredValidator.ValidateAndCollect(
                    request.ProductInformation.ManufacturerId,
                    nameof(request.ProductInformation.ManufacturerId),
                    errors))
                {
                    GuidValidator.ValidateAndCollect(
                        request.ProductInformation.ManufacturerId.Value,
                        nameof(request.ProductInformation.ManufacturerId),
                        errors);
                }

                if (RequiredValidator.ValidateAndCollect(
                    request.ProductInformation.LocalAgentId,
                    nameof(request.ProductInformation.LocalAgentId),
                    errors))
                {
                    GuidValidator.ValidateAndCollect(
                        request.ProductInformation.LocalAgentId.Value,
                        nameof(request.ProductInformation.LocalAgentId),
                        errors);
                }

                EnumValidator.ValidateAndCollect(
                    request.ProductInformation.Template,
                    nameof(request.ProductInformation.Template),
                    errors);

                ImageValidator.Validate(
                    request.ProductInformation.Image,
                    nameof(request.ProductInformation.Image),
                    errors);

                NullableValidator.ValidateRequired(
                    request.ProductInformation.RequiresExpiryTracking,
                    nameof(request.ProductInformation.RequiresExpiryTracking),
                    errors);

                NullableValidator.ValidateRequired(
                    request.ProductInformation.CanBeReturnedIfOpened,
                    nameof(request.ProductInformation.CanBeReturnedIfOpened),
                    errors);

                if (request.ProductInformation.RequiresExpiryTracking == true)
                {
                    RangeValidator.ValidateRange(
                        request.ProductInformation.NearExpiryThresholdInDays,
                         Product.MinDays,
                         Product.MaxDays,
                        nameof(request.ProductInformation.NearExpiryThresholdInDays),
                         errors);
                }

                PackagingInformationValidator.ValidateAndCollect(
                    request.ProductInformation.Template,
                    request.Packaging,
                    errors);

                if (request.UserBarcodes is not null)
                {
                    foreach (var userBarcode in request.UserBarcodes)
                    {
                        EnumValidator.ValidateAndCollect(
                            userBarcode.Unit,
                            nameof(userBarcode.Unit),
                            errors);

                        BarcodeValidator.Validate(
                            userBarcode.Barcode,
                            nameof(userBarcode.Barcode),
                            errors);
                    }

                    var duplicateBarcodes = request.UserBarcodes?
                    .GroupBy(b => b.Barcode?.Value)
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
