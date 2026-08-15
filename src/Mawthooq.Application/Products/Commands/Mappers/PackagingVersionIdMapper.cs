using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Domain.Products.ValueObjects;

namespace Mawthooq.Application.Products.Commands.Mappers
{
    public static class PackagingVersionIdMapper
    {
        public static PackagingVersionId ToDomain(this PackagingVersionIdDto packagingVersionIdDto)
        {
            ArgumentNullException.ThrowIfNull(packagingVersionIdDto, nameof(packagingVersionIdDto));
            return PackagingVersionId.From(packagingVersionIdDto.Value);
        }

        public static PackagingVersionIdDto ToDto(this PackagingVersionId packagingVersionId)
        {
            ArgumentNullException.ThrowIfNull(packagingVersionId, nameof(packagingVersionId));
            return new PackagingVersionIdDto(packagingVersionId.Value);
        }
    }
}
