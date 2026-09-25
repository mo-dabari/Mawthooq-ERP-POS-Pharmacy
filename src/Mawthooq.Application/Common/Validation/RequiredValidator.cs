using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Validation
{
    public static class RequiredValidator
    {
        public static bool ValidateAndCollect<T>(T? value, string fieldName, List<string> errors)
            where T : class
        {
            if (value is null)
            {
                errors.Add($"{fieldName} is required");
                return false;
            }

            return true;
        }
    }
}
