using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.Suppliers.UseCases.IncreaseCurrentBalance
{
    public class IncreaseCurrentBalanceUseCase
    {
        private readonly ISupplierRepository _supplierRepository;
        public IncreaseCurrentBalanceUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public IncreaseCurrentBalanceResponse Execute(IncreaseCurrentBalanceRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            Supplier? supplier = _supplierRepository.GetById(request.SupplierId);

            if (supplier == null)
                throw new InvalidOperationException($"Not Found Supplier ID: {request.SupplierId}");

            supplier.UpdateBalance(request.Balance.ToDomain());

            _supplierRepository.Update(supplier);

            return new IncreaseCurrentBalanceResponse(
                supplier.SupplierId,
                supplier.CurrentBalance.ToDto());
        }
    }
}
