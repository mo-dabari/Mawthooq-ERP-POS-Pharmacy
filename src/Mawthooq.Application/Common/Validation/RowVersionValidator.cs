using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Validation
{
    internal static class RowVersionValidator
    {
        public static void ValidateAndCollect(byte[]? rowVersion, List<string> errors)
        {
            if (rowVersion is null || rowVersion.Length == 0)
                errors.Add("RowVersion is required.");
        }
    }
}
