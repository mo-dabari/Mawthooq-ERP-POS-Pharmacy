using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.Exceptions
{
    public sealed class ConcurrencyConflictException : ConflictException
    {
        public ConcurrencyConflictException(Guid entityId)
            : base($"The record with ID '{entityId}' was modified by another user. Reload and try again.")
        { }

    }
}
