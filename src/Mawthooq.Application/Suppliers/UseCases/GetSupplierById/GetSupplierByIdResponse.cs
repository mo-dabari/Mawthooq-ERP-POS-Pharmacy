using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Suppliers.DTOs;

namespace Mawthooq.Application.Suppliers.UseCases.GetSupplierById
{
    public record class GetSupplierByIdResponse(
        int SupplierId,
        string FullName,
        EmailDto Email,
        AddressDto Address,
        PaymentTermsDto PaymentTerms,
        MoneyDto CreditLimit,
        IReadOnlyList<PhoneDto> Phones,
        bool IsActive
    )
    {

    }
}
