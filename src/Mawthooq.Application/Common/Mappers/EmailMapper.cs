using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Application.Common.DTOs.Mappers
{
    public static class EmailMapper
    {
        public static EmailDto ToDto(this Email email)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));

            return new EmailDto(email.Value);
        }

        public static Email ToDomain(this EmailDto email)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));

            return new Email(email.Value);
        }
    }
}
