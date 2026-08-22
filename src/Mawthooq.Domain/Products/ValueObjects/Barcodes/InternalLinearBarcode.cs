using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Barcodes
{
    public partial record class InternalLinearBarcode : Barcode
    {

        [GeneratedRegex(@"^\d{4,9}$")]
        private static partial Regex InternalLinearRegex();
        private InternalLinearBarcode(string value) : base(value, BarcodeType.Internal)
        {
            if (!InternalLinearRegex().IsMatch(Value))
                throw new ArgumentException("Internal barcode must contain between 4 and 9 digits.");
        }

        public static InternalLinearBarcode New(string value)
        {
            return new InternalLinearBarcode(value);
        }
    }
}
