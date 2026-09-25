using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Validation
{
    internal static class StringValidator
    {
        public static void ValidateAndCollect(string value, string fieldName, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(value))
                errors.Add($"{fieldName} is required.");
        }

        public static void ValidateAndCollect(string value, string fieldName, int maxLength, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(value))
                errors.Add($"{fieldName} is required.");
                
            else if (value.Length > maxLength)
                errors.Add($"{fieldName} must not exceed {maxLength} characters.");
        }
    }
}
