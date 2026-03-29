using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.Mappers;
using Mawthooq.Application.SupplierReturns.DTOs;
using Mawthooq.Domain.SupplierReturns;

namespace Mawthooq.Application.SupplierReturns.Mappers
{
    public static class SupplierReturnMapper
    {
        public static SupplierReturnDto ToDto(this SupplierReturn supplierReturn)
        {
            ArgumentNullException.ThrowIfNull(supplierReturn, nameof(supplierReturn));

            return new SupplierReturnDto(
                supplierReturn.SupplierReturnId,
                supplierReturn.InventoryKey.ToDto(),
                supplierReturn.BatchId,
                supplierReturn.Quantity,
                supplierReturn.Reason,
                supplierReturn.ReturnedAt,
                supplierReturn.Status
            );
        }

        public static IReadOnlyList<SupplierReturnDto> ToDtos(this IReadOnlyList<SupplierReturn> supplierReturns)
        {
            ArgumentNullException.ThrowIfNull(supplierReturns, nameof(supplierReturns));

            return supplierReturns.Select(sr => sr.ToDto()).ToImmutableList();
        }

        public static SupplierReturn ToDomain(this SupplierReturnDto supplierReturnDto, int supplierId)
        {
            ArgumentNullException.ThrowIfNull(supplierReturnDto, nameof(supplierReturnDto));

            return new SupplierReturn(
                supplierReturnDto.SupplierReturnId,
                supplierId,
                supplierReturnDto.InventoryKey.ToDomain(),
                supplierReturnDto.BatchId,
                supplierReturnDto.Quantity,
                supplierReturnDto.Reason,
                supplierReturnDto.ReturnedAt,
                supplierReturnDto.Status
            );
        }

    }
}
