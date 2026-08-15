using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Manufacturers.DTOs;
using Mawthooq.Domain.Manufacturers.ValueObjects;

namespace Mawthooq.Application.Manufacturers.Mappers
{
    public static class ManufacturerIdMapper
    {
                public static ManufacturerId ToDomain(this ManufacturerIdDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            return ManufacturerId.From(dto.Value);
        }

        public static ManufacturerIdDto ToDto(this ManufacturerId manufacturerId)
        {
            ArgumentNullException.ThrowIfNull(manufacturerId, nameof(manufacturerId));

            return new ManufacturerIdDto(manufacturerId.Value);
        }
    }
}
