using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Money.Enums;

namespace Mawthooq.Application.Common.DTOs
{
    public record class MoneyDto(
        decimal Amount,
        Currency Currency
    )
    {
    }
}
