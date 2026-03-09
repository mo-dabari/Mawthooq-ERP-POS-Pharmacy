using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects;

namespace Mawthooq.Application.Common.DTOs.Mappers
{
    public static class PhoneMapper
    {
        public static PhoneDto ToDto(this Phone phone)
        {
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            return new PhoneDto(phone.CountryCode, phone.Value);
        }

        public static Phone ToDomain(this PhoneDto phoneDto)
        {
            ArgumentNullException.ThrowIfNull(phoneDto, nameof(phoneDto));
            return new Phone(phoneDto.CountryCode, phoneDto.Number);
        }
    }
}
