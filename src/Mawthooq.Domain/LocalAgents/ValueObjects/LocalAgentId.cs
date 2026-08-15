using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.LocalAgents.ValueObjects
{
    public sealed record LocalAgentId : DomainObjectId
    {
        private LocalAgentId(Guid value) : base(value)
        {
        }
        public static LocalAgentId New() => new(Guid.NewGuid());
        public static LocalAgentId From(Guid value) => new(value);
    }
}
