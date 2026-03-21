using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Sales.DTOs;
using Mawthooq.Application.Sales.Mappers;
using Mawthooq.Domain.Sales;

namespace Mawthooq.Application.Sales.UseCases.CreateSale
{
    public class CreateSaleUseCase
    {
        private readonly ISaleRepository _saleRepository;
        public CreateSaleUseCase(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public CreateSaleResponse Execute(CreateSaleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Sale newSale = Sale.Create(request.BranchId, request.Currency);

            foreach (CreateSaleLineRequest SaleLine in request.SaleLines)
            {
                newSale.AddLine(
                    SaleLine.ProductId,
                    SaleLine.Quantity,
                    SaleLine.SellingPrice.ToDomain(),
                    SaleLine.PurchaseCost.ToDomain());
            }

            Sale? saveSale = _saleRepository.Add(newSale);

            if (saveSale is null)
                throw new InvalidOperationException("Failed to create Sale.");


            return new CreateSaleResponse(
                SaleId: saveSale.SaleId,
                BranchId: saveSale.BranchId,
                Currency: saveSale.Currency,
                SaleDate: saveSale.SaleDate,
                Status: saveSale.Status,
                Lines: saveSale.Lines.Select(line => line.ToDto()).ToImmutableList()
            );
        }
    }
}
