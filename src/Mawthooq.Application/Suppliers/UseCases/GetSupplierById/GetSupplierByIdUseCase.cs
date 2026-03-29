using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Suppliers.Mappers;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.Suppliers.UseCases.GetSupplierById
{
    public class GetSupplierByIdUseCase
    {
        private readonly ISupplierRepository _supplierRepository;
        public GetSupplierByIdUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public GetSupplierByIdResponse Execute(GetSupplierByIdRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Supplier? supplier = _supplierRepository.GetById(request.SupplierId);

            if (supplier == null)
                throw new InvalidOperationException($"Not Found Supplier ID:{request.SupplierId}");

            return new GetSupplierByIdResponse(
                supplier.SupplierId,
                supplier.FullName,
                supplier.Email.ToDto(),
                supplier.Address.ToAddressDto(),
                supplier.PaymentTerms.ToDto(),
                supplier.CreditLimit.ToDto(),
                supplier.Phones.Select(p => p.ToDto()).ToImmutableList(),
                supplier.IsActive

            );
        }
    }
}
