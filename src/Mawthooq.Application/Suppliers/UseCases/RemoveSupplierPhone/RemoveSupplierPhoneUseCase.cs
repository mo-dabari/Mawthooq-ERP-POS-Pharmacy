using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.Suppliers.UseCases.RemoveSupplierPhone
{
    public class RemoveSupplierPhoneUseCase
    {
        private readonly ISupplierRepository _supplierRepository;
        public RemoveSupplierPhoneUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public RemoveSupplierPhoneResponse Execute(RemoveSupplierPhoneRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Supplier? supplier = _supplierRepository.GetById(request.SupplierId);

            if (supplier == null)
                throw new InvalidOperationException($"Not Found Supplier ID: {request.SupplierId}");

            supplier.RemovePhone(request.Phone.ToDomain());
            _supplierRepository.Update(supplier);

            return new RemoveSupplierPhoneResponse(supplier.SupplierId, request.Phone);
        }
    }
}
