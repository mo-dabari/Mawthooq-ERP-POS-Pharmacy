using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.ManufacturersAgencys.Enums;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Domain.ManufacturersAgencys
{
    public class ManufacturerAgency
    {
        public ManufacturerId ManufacturerId { get; }
        public LocalAgentId LocalAgentId { get; }
        public CountryCode CountryCode { get; private set; }
        public AgencyType Type { get; private set; }
        public Status Status { get; private set; }

        public ManufacturerAgency(
            ManufacturerId manufacturerId,
            LocalAgentId localAgentId,
            CountryCode countryCode,
            AgencyType type
        )
        {

            ArgumentNullException.ThrowIfNull(manufacturerId, nameof(manufacturerId));
            ArgumentNullException.ThrowIfNull(localAgentId, nameof(localAgentId));

            ManufacturerId = manufacturerId;
            LocalAgentId = localAgentId;
            CountryCode = countryCode;
            Type = type;
            Status = Status.Active;
        }
    }
}
