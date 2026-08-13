using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Barcodes
{
    public static class UnitBarcodeFactory
    {
        public static Barcode Create(string value, BarcodeType barcodeType)
        {
            return barcodeType switch
            {
                BarcodeType.EAN13 =>
                     BarcodeEAN13Linear.New(value),

                BarcodeType.Internal =>
                    InternalLinearBarcode.New(value),

                BarcodeType.QRCode =>
                    BarcodeQR.New(value),

                _ => throw new NotSupportedException($"Barcode Type{barcodeType} is not supported.")

            };
        }
    }
}
