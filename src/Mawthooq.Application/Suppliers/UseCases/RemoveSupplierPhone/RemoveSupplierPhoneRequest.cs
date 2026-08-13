using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Suppliers.UseCases.RemoveSupplierPhone
{
    public record class RemoveSupplierPhoneRequest(int SupplierId, PhoneDto Phone)
    {

    }
}
