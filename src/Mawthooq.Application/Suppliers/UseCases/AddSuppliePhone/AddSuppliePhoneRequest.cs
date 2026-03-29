using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Suppliers.UseCases.AddSuppliePhone
{
    public record class AddSuppliePhoneRequest(int SupplierId, PhoneDto Phone)
    {

    }
}
