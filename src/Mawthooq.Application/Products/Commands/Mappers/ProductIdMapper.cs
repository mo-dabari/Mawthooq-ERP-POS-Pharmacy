using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.ValueObjects;

namespace Mawthooq.Application.Products.Commands.Mappers
{
    public static class ProductIdMapper
    {
        public static ProductId ToDomain(this ProductIdDto productIdDto)
        {
            ArgumentNullException.ThrowIfNull(productIdDto, nameof(productIdDto));

            return ProductId.From(productIdDto.Value);
        }

        public static ProductIdDto ToDto(this ProductId productId)
        {
            ArgumentNullException.ThrowIfNull(productId, nameof(productId));

            return new ProductIdDto(productId.Value);
        }
    }
}
