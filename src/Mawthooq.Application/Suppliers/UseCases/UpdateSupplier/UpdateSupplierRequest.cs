using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;
using Mawthooq.Application.Suppliers.DTOs;

namespace Mawthooq.Application.Suppliers.UseCases.UpdateSupplier
{
    public record class UpdateSupplierRequest(
        int SupplierId,
        string FirstName,
        string LastName,
        EmailDto Email,
        AddressDto Address,
        MoneyDto CreditLimit,
        byte[] rowVersion
    )
    {

    }
}
