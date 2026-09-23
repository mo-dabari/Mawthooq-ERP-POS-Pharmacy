using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Exceptions
{
    /// <summary>
    /// Base type for "the requested entity does not exist" failures.
    /// The Presentation layer's Global Exception Handler maps any exception
    /// deriving from this type to HTTP 404 — new NotFound exceptions never
    /// require a middleware change.
    /// </summary>

    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message)
        { }

    }
}
