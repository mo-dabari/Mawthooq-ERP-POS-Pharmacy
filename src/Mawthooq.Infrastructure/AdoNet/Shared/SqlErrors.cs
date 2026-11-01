using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Shared
{
    internal static class SqlErrors
    {
        internal static bool IsUniqueViolation(SqlException ex, string indexName) =>
            ex.Number is 2601 or 2627 &&
            ex.Message.Contains(indexName, StringComparison.OrdinalIgnoreCase);
    }
}
