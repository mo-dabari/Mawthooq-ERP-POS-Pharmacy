using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
