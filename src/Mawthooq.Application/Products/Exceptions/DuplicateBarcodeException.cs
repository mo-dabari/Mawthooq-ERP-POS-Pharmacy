using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;

namespace Mawthooq.Application.Products.Exceptions
{
    public class DuplicateBarcodeException : ConflictException
    {
        public DuplicateBarcodeException(string barcodeValue)
            : base($"Barcode '{barcodeValue}' already exists.")
        {
        }

    }
}
