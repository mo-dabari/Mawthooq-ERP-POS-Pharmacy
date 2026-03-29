using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Suppliers.DTOs;

namespace Mawthooq.Application.Suppliers.UseCases.CreateSupplier
{
    public record class CreateSupplierRequest(
        string FirstName,
        string LastName ,
        EmailDto Email,
        AddressDto Address,
        PaymentTermsDto PaymentTerms,
        MoneyDto CreditLimit,
        IReadOnlyList<PhoneDto> Phones

    )
    {

    }
}
