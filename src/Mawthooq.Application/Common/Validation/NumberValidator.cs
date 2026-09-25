using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Validation
{
    internal static class NumberValidator
    {
        public static void ValidateNonNegative(int value, string fieldName, List<string> errors)
        {
            if (value < 0)
                errors.Add($"{fieldName} cannot be negative.");
        }

        public static void ValidatePositive(int? value, string fieldName, List<string> errors)
        {
            if (value is null || value <= 0)
                errors.Add($"{fieldName} must be greater than zero.");
        }
    }
}
