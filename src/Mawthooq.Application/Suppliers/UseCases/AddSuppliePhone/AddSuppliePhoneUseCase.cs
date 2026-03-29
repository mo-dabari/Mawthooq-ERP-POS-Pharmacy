using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.Suppliers.UseCases.AddSuppliePhone
{
    public class AddSuppliePhoneUseCase
    {
        private readonly ISupplierRepository _supplierRepository;
        public AddSuppliePhoneUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public AddSuppliePhoneResponse Execute(AddSuppliePhoneRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Supplier? supplier = _supplierRepository.GetById(request.SupplierId);

            if (supplier == null)
                throw new InvalidOperationException($"Not Found Supplier ID: {request.SupplierId}");

            supplier.AddPhone(request.Phone.ToDomain());

            _supplierRepository.Update(supplier);

            return new AddSuppliePhoneResponse(supplier.SupplierId, request.Phone);
        }
    }
}
