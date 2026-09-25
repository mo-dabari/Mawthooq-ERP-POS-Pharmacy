using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Validation
{
    internal static class RangeValidator
    {
        public static void ValidateMin<T>(T value, T min, string fieldName, List<string> errors)
                    where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                errors.Add($"{fieldName} must be greater than or equal to {min}.");
        }

        public static void ValidateRange<T>(T value, T min, T max, string fieldName, List<string> errors)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                errors.Add($"{fieldName} must be between {min} and {max}.");
        }
    }
}
