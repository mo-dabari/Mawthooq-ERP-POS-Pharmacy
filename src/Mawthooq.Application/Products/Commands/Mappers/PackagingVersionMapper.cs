using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Products.Commands.DTOs;
using Mawthooq.Application.Products.Commands.UseCases.CreateProduct;
using Mawthooq.Domain.Products.Entities;

namespace Mawthooq.Application.Products.Commands.Mappers
{
    public static class PackagingVersionMapper
    {
        public static PackagingVersionDto ToDto(this PackagingVersion packagingVersion)
        {
            ArgumentNullException.ThrowIfNull(packagingVersion, nameof(packagingVersion));

            return new PackagingVersionDto(
                packagingVersion.PackagingVersionId.ToDto(),
                packagingVersion.Version,
                packagingVersion.Status,
                packagingVersion.Units.Select(u => u.ToDto()).ToList());
        }
    }
}
