using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Inventories.UseCases.ReturnStockToSupplier;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.Suppliers.UseCases.DeductCurrentBalance
{
    public class DeductCurrentBalanceUseCase
    {
        private readonly ISupplierRepository _supplierRepository;
        public DeductCurrentBalanceUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public DeductCurrentBalanceResponse Execute(DeductCurrentBalanceRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            
            Supplier? supplier = _supplierRepository.GetById(request.SupplierId);

            if (supplier == null)
                throw new InvalidOperationException($"Not Found Supplier ID: {request.SupplierId}");

            supplier.DeductBalance(request.Balance.ToDomain());

            _supplierRepository.Update(supplier);

            return new DeductCurrentBalanceResponse(
                supplier.SupplierId,
                 supplier.CurrentBalance.ToDto());
        }
    }
}
