using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Suppliers.Enums;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.Suppliers.UseCases.UpdateSupplierStatus
{
    public class UpdateSupplierStatusUseCase
    {
        private readonly ISupplierRepository _supplierRepository;
        public UpdateSupplierStatusUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public UpdateSupplierStatusResponse Execute(UpdateSupplierStatusRequest request)
        {
            Supplier? supplier = _supplierRepository.GetById(request.SupplierId);

            if (supplier == null)
                throw new InvalidOperationException($"Not Found Supplier ID: {request.SupplierId}");

            if (request.Status == SupplierStatus.Active)
                supplier.Activate();

            else if (request.Status == SupplierStatus.Inactive)
                supplier.Deactivate();

            _supplierRepository.Update(supplier);

            return new UpdateSupplierStatusResponse(
                supplier.SupplierId,
                supplier.IsActive ? SupplierStatus.Active : SupplierStatus.Inactive
                );
        }
    }
}
