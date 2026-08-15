using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.LocalAgents.DTOs;
using Mawthooq.Application.Manufacturers.DTOs;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;

namespace Mawthooq.Application.LocalAgents.Mappers
{
    public static class LocalAgentIdMapper
    {
        public static LocalAgentId ToDomain(this LocalAgentIdDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            return LocalAgentId.From(dto.Value);
        }

        public static LocalAgentIdDto ToDto(this LocalAgentId localAgentId)
        {
            ArgumentNullException.ThrowIfNull(localAgentId, nameof(localAgentId));

            return new LocalAgentIdDto(localAgentId.Value);
        }
    }
}
