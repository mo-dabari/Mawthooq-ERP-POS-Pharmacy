using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Barcodes
{
    public partial record class BarcodeEAN13Linear : Barcode
    {

        [GeneratedRegex(@"^\d{13}$")]
        private static partial Regex Ean13Regex();

        private BarcodeEAN13Linear(string value) : base(value, BarcodeType.EAN13)
        {
            if (!Ean13Regex().IsMatch(Value))
                throw new ArgumentException("Invalid Barcode 'Ean13' Must Be 13 Digits Only");
        }

        public static BarcodeEAN13Linear New(string value)
        {
            return new BarcodeEAN13Linear(value);
        }
    }
}
