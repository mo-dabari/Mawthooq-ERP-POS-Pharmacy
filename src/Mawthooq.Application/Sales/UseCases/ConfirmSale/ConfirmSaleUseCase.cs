using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.ValueObjects;
using Mawthooq.Domain.Sales;
using Mawthooq.Domain.Sales.Entity;

namespace Mawthooq.Application.Sales.UseCases.ConfirmSale
{
    public class ConfirmSaleUseCase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISaleRepository _saleRepository;
        public ConfirmSaleUseCase(IInventoryRepository inventoryRepository, ISaleRepository saleRepository)
        {
            _inventoryRepository = inventoryRepository;
            _saleRepository = saleRepository;
        }

        public ConfirmSaleResponse Execute(ConfirmSaleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Sale? sale = _saleRepository.GetById(request.SaleId);
            if (sale == null)
                throw new InvalidOperationException("Sale not found");

            foreach (SaleLine line in sale.Lines)
            {
                Inventory? inventoryLine = _inventoryRepository.GetByInventoryKey(new InventoryKey(sale.BranchId, line.ProductId));
                if (inventoryLine == null)
                    throw new InvalidOperationException($"Inventory line not found for product {line.ProductId} in branch {sale.BranchId}");

                if (inventoryLine.GetAvailableQuantity() < line.Quantity)
                    throw new InvalidOperationException($"Insufficient inventory for product {line.ProductId} in branch {sale.BranchId}");

                inventoryLine.DeductStock(line.Quantity);
                _inventoryRepository.Update(inventoryLine);
            }

            sale.Confirm();
            _saleRepository.Update(sale);

            return new ConfirmSaleResponse(sale.SaleId, sale.BranchId, sale.TotalAmount.ToDto());
        }
    }
}
