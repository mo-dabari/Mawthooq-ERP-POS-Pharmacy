using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Inventories.Mappers;
using Mawthooq.Application.Inventories.UseCases.ReturnStockToSupplier.DTOs;
using Mawthooq.Domain.SupplierReturns;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.Inventories.UseCases.ReturnStockToSupplier.Mappers
{
    public static class SupplierReturnMapper
    {
        public static SupplierReturnDto ToDto(this SupplierReturn supplierReturn)
        {
            ArgumentNullException.ThrowIfNull(supplierReturn, nameof(supplierReturn));

            return new SupplierReturnDto(
                supplierReturn.SupplierReturnId,
                supplierReturn.SupplierId,
                supplierReturn.InventoryKey.ToDto(),
                supplierReturn.BatchId,
                supplierReturn.Quantity,
                supplierReturn.Reason,
                supplierReturn.ReturnedAt,
                supplierReturn.Status
            );
        }

        public static SupplierReturn ToDomain(this SupplierReturnDto supplierReturnDto)
        {
            ArgumentNullException.ThrowIfNull(supplierReturnDto, nameof(supplierReturnDto));

            return new SupplierReturn(
                supplierReturnDto.SupplierReturnId,
                supplierReturnDto.SupplierId,
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
