using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Customers.UseCases.UpdateCustomer
{
    public record class UpdateCustomerRequest(int CustomerId, PhoneDto PhoneNumber, bool IsActive)
    {

    }
}
