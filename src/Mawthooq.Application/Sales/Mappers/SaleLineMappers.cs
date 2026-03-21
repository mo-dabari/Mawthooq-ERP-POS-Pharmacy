using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Sales.DTOs;
using Mawthooq.Domain.Sales;
using Mawthooq.Domain.Sales.Entity;

namespace Mawthooq.Application.Sales.Mappers
{
    public static class SaleLineMappers
    {
        public static SaleLine ToDomain(this SaleLineDto saleLineDto)
        {
            ArgumentNullException.ThrowIfNull(saleLineDto, nameof(saleLineDto));

            return new SaleLine(
                saleLineDto.SaleLineId,
                saleLineDto.ProductId,
                saleLineDto.Quantity,
                saleLineDto.SellingPrice.ToDomain(),
                saleLineDto.PurchaseCost.ToDomain());
        }

        public static SaleLineDto ToDto(this SaleLine saleLine)
        {
            ArgumentNullException.ThrowIfNull(saleLine, nameof(saleLine));

            return new SaleLineDto(
                saleLine.SaleLineId,
                saleLine.ProductId,
                saleLine.Quantity,
                saleLine.SellingPrice.ToDto(),
                saleLine.PurchaseCost.ToDto(),
                saleLine.TotalAmount.ToDto(),
                saleLine.TotalPurchaseCost.ToDto(),
                saleLine.GrossProfit.ToDto());
        }
    }
}
