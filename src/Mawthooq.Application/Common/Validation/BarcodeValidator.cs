using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Application.Common.Validation
{
    internal static class BarcodeValidator
{
    internal static void Validate(BarcodeDto barcode, string name, List<string> errors)
    {
        if (RequiredValidator.ValidateAndCollect(barcode, name, errors))
        {
            StringValidator.ValidateAndCollect(barcode.Value, $"{name}.{nameof(barcode.Value)}", Barcode.MaxValueLength, errors);
            EnumValidator.ValidateAndCollect(barcode.Type, $"{name}.{nameof(barcode.Type)}", errors);
        }
    }
}
}
