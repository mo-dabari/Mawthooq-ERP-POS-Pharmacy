using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Common.Interfaces;

namespace Mawthooq.Domain.Customer
{
    public interface ICustomerRepository : IBaseRepository<Customer, int>
    {

    }
}
