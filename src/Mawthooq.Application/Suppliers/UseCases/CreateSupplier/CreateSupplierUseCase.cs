using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Suppliers.Mappers;
using Mawthooq.Domain.Suppliers;

namespace Mawthooq.Application.Suppliers.UseCases.CreateSupplier
{
    public class CreateSupplierUseCase
    {
        private readonly ISupplierRepository _supplierRepository;
        public CreateSupplierUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public CreateSupplierResponse Execute(CreateSupplierRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Supplier newSupplier = Supplier.Create(
                request.FirstName,
                request.LastName,
                request.Email.ToDomain(),
                request.Address.ToAddressDomain(),
                request.PaymentTerms.ToDomain(),
                request.CreditLimit.ToDomain()
                );

            Supplier? SaveSupplier = _supplierRepository.Add(newSupplier);

            if (SaveSupplier is null)
                throw new InvalidOperationException("Failed to create supplier");

            return new CreateSupplierResponse(
                SaveSupplier.SupplierId,
                SaveSupplier.FullName,
                SaveSupplier.Email.ToDto(),
                SaveSupplier.Address.ToAddressDto(),
                SaveSupplier.PaymentTerms.ToDto(),
                SaveSupplier.CreditLimit.ToDto(),
                SaveSupplier.Phones.Select(p => p.ToDto()).ToImmutableList(),
                SaveSupplier.IsActive
            );
        }
    }
}
