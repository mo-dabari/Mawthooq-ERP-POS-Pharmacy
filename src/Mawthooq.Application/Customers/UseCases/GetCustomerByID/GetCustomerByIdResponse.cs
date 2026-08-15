using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Customers.UseCases.GetCustomerByID
{
    public record class GetCustomerByIdResponse(int CustomerId, string FullName, bool IsActive, DateTime CreatedAt)
    {

    }
}
