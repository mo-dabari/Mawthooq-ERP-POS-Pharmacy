using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Validation
{
    internal static class EnumValidator
    {
        public static void ValidateAndCollect<TEnum>(TEnum value, string fieldName, List<string> errors)
            where TEnum : struct, Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                errors.Add($"The {fieldName} '{value}' is invalid.");
            }
        }

        public static void ValidateAndCollect<TEnum>(TEnum? value, string fieldName, List<string> errors)
            where TEnum : struct, Enum
        {
            if (value is null || !Enum.IsDefined(typeof(TEnum), value.Value))
                errors.Add($"The {fieldName} '{value}' is invalid.");
        }
    }
}
