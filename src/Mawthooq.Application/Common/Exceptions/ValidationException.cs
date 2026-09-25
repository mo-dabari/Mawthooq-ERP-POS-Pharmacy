using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public IReadOnlyList<string> Errors { get; }

        public ValidationException(IReadOnlyList<string> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
