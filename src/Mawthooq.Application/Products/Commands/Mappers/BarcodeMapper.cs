using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Application.Products.Commands.Mappers
{
    public static class BarcodeMapper
    {
        public static Barcode ToDomain(this BarcodeDto barcodeDto)
        {
            ArgumentNullException.ThrowIfNull(barcodeDto, nameof(barcodeDto));

            return UnitBarcodeFactory.Create(barcodeDto.Value, barcodeDto.Type);
        }

        public static BarcodeDto ToDto(this Barcode barcode)
        {
            ArgumentNullException.ThrowIfNull(barcode, nameof(barcode));

            return new BarcodeDto(barcode.Value, barcode.Type);
        }
    }
}
