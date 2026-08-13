using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Sales.DTOs;
using Mawthooq.Domain.Sales;
using Mawthooq.Domain.Sales.Entity;

namespace Mawthooq.Application.Sales.UseCases.GetSaleById
{
    public class GetSaleByIdUseCase
    {
        private readonly ISaleRepository _saleRepository;
        public GetSaleByIdUseCase(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public GetSaleByIdResponse Execute(GetSaleByIdRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Sale? sale = _saleRepository.GetById(request.SaleId);
            if (sale == null)
                throw new InvalidOperationException("Sale not found.");

            IReadOnlyList<SaleLineDto>? saleLinesDto = sale.Lines.Select(line => new SaleLineDto(
                line.SaleLineId,
                line.ProductId,
                line.Quantity,
                line.SellingPrice.ToDto(),
                line.PurchaseCost.ToDto(),
                line.TotalAmount.ToDto(),
                line.TotalPurchaseCost.ToDto(),
                line.GrossProfit.ToDto()
            )).ToImmutableList();

            return new GetSaleByIdResponse(
                sale.SaleId,
                sale.BranchId,
                saleLinesDto,
                sale.Currency
            );
        }
    }
}
