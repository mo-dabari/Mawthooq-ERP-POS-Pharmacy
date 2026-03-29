using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.SupplierReturns.Mappers;
using Mawthooq.Domain.SupplierReturns;

namespace Mawthooq.Application.SupplierReturns.UseCases.GetSupplierReturnsBySupplierId
{
    public class GetSupplierReturnsBySupplierIdUseCase
    {
        private readonly ISupplierReturnRepository _supplierReturnRepository;
        public GetSupplierReturnsBySupplierIdUseCase(ISupplierReturnRepository supplierReturnRepository)
        {
            ArgumentNullException.ThrowIfNull(supplierReturnRepository, nameof(supplierReturnRepository));
            _supplierReturnRepository = supplierReturnRepository;
        }

        public GetSupplierReturnsBySupplierIdResponse Execute(GetSupplierReturnsBySupplierIdRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var supplierReturns = _supplierReturnRepository.GetSupplierReturns(request.SupplierId);
            if (supplierReturns is null || !supplierReturns.Any())
                throw new InvalidOperationException($"No supplier return found with id {request.SupplierId}");

            return new GetSupplierReturnsBySupplierIdResponse(request.SupplierId, supplierReturns.ToDtos());
        }
    }
}
