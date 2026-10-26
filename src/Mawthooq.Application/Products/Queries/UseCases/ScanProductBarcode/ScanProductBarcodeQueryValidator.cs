using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Common.Validation;

namespace Mawthooq.Application.Products.Queries.UseCases.ScanProductBarcode
{
    public static class ScanProductBarcodeQueryValidator
    {
        public static void Validate(ScanProductBarcodeQuery query)
        {
            List<string> errors = new();

            StringValidator.ValidateAndCollect(query.Barcode, nameof(query.Barcode), errors);

            if (errors.Count > 0) throw new ValidationException(errors);
        }
    }
}
