using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Application.Products.Commands.DTOs
{
    public record class ProductUnitDetailsDto
    (
        UnitOfMeasure Unit,
        int ConversionRate,
        UnitRank Rank,
        IReadOnlyList<BarcodeDto> Barcodes
    );
}
