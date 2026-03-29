using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Suppliers;
using Mawthooq.Domain.ValueObjects;

namespace Mawthooq.Application.Suppliers.UseCases.UpdateSupplier
{
    public class UpdateSupplierUseCase
    {
        private readonly ISupplierRepository _supplierRepository;
        public UpdateSupplierUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public UpdateSupplierResponse Execute(UpdateSupplierRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Supplier? supplier = _supplierRepository.GetById(request.SupplierId);

            if (supplier == null)
                throw new InvalidOperationException("Not Found Supplier");

            supplier.UpdateName(request.FirstName, request.LastName);
            supplier.UpdateEmail(request.Email.ToDomain());
            supplier.UpdateAddress(request.Address.ToAddressDomain());

            if (request.CreditLimit.Amount < supplier.CurrentBalance.Amount)
                throw new InvalidOperationException("Must Be Credit Limit Bigger Then Or Equle Current Balance");
            supplier.UpdateCreditLimit(request.CreditLimit.ToDomain());

            _supplierRepository.Update(supplier);

            return new UpdateSupplierResponse(
                supplier.SupplierId,
                supplier.FullName,
                supplier.Email.ToDto(),
                supplier.Address.ToAddressDto(),
                supplier.CreditLimit.ToDto()
            );
        }
    }
}
