using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Barcodes
{
    public partial record class BarcodeQR : Barcode
    {

        [GeneratedRegex(@"^[\x20-\x7E]+$")]
        private static partial Regex AsciiPrintableRegex();
        private BarcodeQR(string value) : base(value, BarcodeType.QRCode)
        {
            if (!AsciiPrintableRegex().IsMatch(Value))
                throw new ArgumentException("Invalid Barcode 'QR' It must contain printable characters, letters, or numbers");
        }

        public static BarcodeQR New(string value)
        {
            return new BarcodeQR(value);
        }
    }
}
