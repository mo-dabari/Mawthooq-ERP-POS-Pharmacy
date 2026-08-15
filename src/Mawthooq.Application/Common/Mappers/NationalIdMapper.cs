using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Application.Common.DTOs.Mappers
{
    public static class NationalIdMapper
    {
        public static NationalIdDto ToDto(this NationalId nationalId)
        {
            ArgumentNullException.ThrowIfNull(nationalId, nameof(nationalId));
            return new NationalIdDto(nationalId.CountryCode, nationalId.Value);
        }

        public static NationalId ToDomain(this NationalIdDto nationalIdDto)
        {
            ArgumentNullException.ThrowIfNull(nationalIdDto, nameof(nationalIdDto));
            return new NationalId(nationalIdDto.CountryCode, nationalIdDto.Value);
        }
    }
}
