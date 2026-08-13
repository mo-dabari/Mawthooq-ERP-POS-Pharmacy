using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Sales;

namespace Mawthooq.Application.Sales.UseCases.AddSaleLine
{
    public class AddSaleLineUseCase
    {
        private readonly ISaleRepository _saleRepository;
        public AddSaleLineUseCase(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public AddSaleLineResponse Execute(AddSaleLineRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Sale? sale = _saleRepository.GetById(request.SaleId);

            if (sale is null)
                throw new InvalidOperationException($"Sale with id {request.SaleId} not found.");

            sale.AddLine(request.ProductId, request.Quantity, request.SellingPrice.ToDomain(), request.PurchaseCost.ToDomain());

            _saleRepository.Update(sale);

            return new AddSaleLineResponse(request.SaleId, request.ProductId, request.Quantity, request.SellingPrice, request.PurchaseCost);
        }
    }
}
