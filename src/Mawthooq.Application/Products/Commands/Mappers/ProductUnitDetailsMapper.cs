using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Domain.Products.ValueObjects;

namespace Mawthooq.Application.Products.Commands.Mappers
{
    public static class ProductUnitDetailsMapper
    {
        public static ProductUnitDetailsDto ToDto(this ProductUnitDetails unit)
        {
            ArgumentNullException.ThrowIfNull(unit, nameof(unit));

            return new ProductUnitDetailsDto(
                unit.Unit,
                unit.ConversionRate,
                unit.Rank,
                unit.Barcodes.Select(b => b.ToDto()).ToList());
        }
    }
}
