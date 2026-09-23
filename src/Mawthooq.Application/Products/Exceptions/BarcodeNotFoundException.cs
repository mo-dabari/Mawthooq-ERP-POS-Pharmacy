using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Application.Products.Exceptions
{
    public sealed class BarcodeNotFoundException : NotFoundException
    {
        public BarcodeNotFoundException(string barcodeValue)
            : base($"Barcode '{barcodeValue}' not found.")
        {
        }
    }
}
