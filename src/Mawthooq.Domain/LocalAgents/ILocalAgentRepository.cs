using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.LocalAgents.ValueObjects;

namespace Mawthooq.Domain.LocalAgents
{
    public interface ILocalAgentRepository
    {
        Task<bool> ExistsAndActiveAsync(LocalAgentId localAgentId, CancellationToken cancellationToken = default);

    }
}
