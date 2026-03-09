using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Enums;

namespace Mawthooq.Application.Common.DTOs
{
    public record class PhoneDto (CountryCode CountryCode, string Number)
    {

    }
}
