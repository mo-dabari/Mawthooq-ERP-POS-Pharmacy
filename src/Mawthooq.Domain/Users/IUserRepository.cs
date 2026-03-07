using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Common.Interfaces;

namespace Mawthooq.Domain.Users
{
    public interface IUserRepository : IBaseRepository<User, int>
    {

    }
}
