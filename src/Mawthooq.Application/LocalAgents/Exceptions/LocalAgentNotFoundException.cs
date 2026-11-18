using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;

namespace Mawthooq.Application.LocalAgents.Exceptions
{
    public sealed class LocalAgentNotFoundException : NotFoundException
    {
        public LocalAgentNotFoundException(Guid localAgentId)
            : base($"Local Agent with id '{localAgentId}' was not found or is inactive.") { }
    }
}
