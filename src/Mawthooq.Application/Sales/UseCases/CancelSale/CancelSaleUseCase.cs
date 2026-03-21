using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Sales;

namespace Mawthooq.Application.Sales.UseCases.CancelSale
{
    public class CancelSaleUseCase
    {
        private readonly ISaleRepository _saleRepository;
        public CancelSaleUseCase(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public CancelSaleResponse Execute(CancelSaleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            Sale? sale = _saleRepository.GetById(request.SaleId);

            if (sale == null)
                throw new InvalidOperationException("Sale not found.");

            sale.Cancel();
            _saleRepository.Update(sale);

            return new CancelSaleResponse(sale.SaleId);
        }
    }
}
