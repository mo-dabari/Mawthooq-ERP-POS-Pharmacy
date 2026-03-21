using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Sales;
using Mawthooq.Domain.Sales.Entity;

namespace Mawthooq.Application.Sales.UseCases.RemoveSaleLine
{
    public class RemoveSaleLineUseCase
    {
        private readonly ISaleRepository _saleRepository;
        public RemoveSaleLineUseCase(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }
        public RemoveSaleLineResponse Execute(RemoveSaleLineRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Sale? sale = _saleRepository.GetById(request.SaleId);

            if (sale is null)
                throw new InvalidOperationException($"Sale with id {request.SaleId} not found.");

            SaleLine? saleLine = sale.Lines.FirstOrDefault(line => line.SaleLineId == request.SaleLineId);

            if (saleLine is null)
                throw new InvalidOperationException($"Sale line with id {request.SaleLineId} not found in sale with id {request.SaleId}.");

            sale.RemoveLine(request.SaleLineId);

            _saleRepository.Update(sale);

            return new RemoveSaleLineResponse(sale.SaleId, request.SaleLineId);
        }
    }
}
