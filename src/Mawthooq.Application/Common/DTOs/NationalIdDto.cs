using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;

namespace Mawthooq.Application.Common.DTOs
{
    public record class NationalIdDto(CountryCode CountryCode, string Value);
}
