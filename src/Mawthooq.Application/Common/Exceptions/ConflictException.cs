using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Exceptions
{
    /// <summary>
    /// Base type for "the request conflicts with the current state" failures
    /// (duplicates, concurrency conflicts). Mapped to HTTP 409 by the
    /// Global Exception Handler.
    /// </summary>

    public abstract class ConflictException : Exception
    {
        protected ConflictException(string message) : base(message)
        { }
    }
}
