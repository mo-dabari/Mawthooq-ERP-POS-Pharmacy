using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Validation
{
    internal static class GuidValidator
    {
        public static void ValidateAndCollect(Guid value, string fieldName, List<string> errors)
        {
            if (value == Guid.Empty)
                errors.Add($"{fieldName} is required.");
        }
    }
}
