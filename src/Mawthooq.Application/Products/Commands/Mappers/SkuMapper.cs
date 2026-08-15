using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;

namespace Mawthooq.Application.Products.Commands.Mappers
{
    public static class SkuMapper
    {
        public static SKU ToDomain(this SkuDto skuDto)
        {
            return SKU.Create(skuDto.Value);
        }

        public static SkuDto ToDto(this SKU sKU)
        {
            return new SkuDto(sKU.Value);
        }
    }
}
