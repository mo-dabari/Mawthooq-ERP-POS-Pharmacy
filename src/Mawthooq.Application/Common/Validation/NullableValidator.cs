using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Validation
{
    internal static class NullableValidator
    {
        internal static bool ValidateRequired<T>(T? value, string name, List<string> errors) where T : struct
        {
            if (value.HasValue)
                return true;

            errors.Add($"{name} is required.");
            return false;
        }
    }
}
