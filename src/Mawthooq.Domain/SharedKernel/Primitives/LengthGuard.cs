using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.Primitives
{
    internal static class LengthGuard
    {

        public static void MaxLength(string value, int maxLength, string paramName)
        {
            if (value.Length > maxLength)
                throw new ArgumentException($"{paramName} must not exceed {maxLength} characters.", paramName);

        }
    }
}
