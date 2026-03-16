using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Money;

namespace Mawthooq.Application.Common.DTOs.Mappers
{
    public static class MoneyMapper
    {
        public static MoneyDto ToDto(this Money money)
        {
            ArgumentNullException.ThrowIfNull(money, nameof(money));

            return new MoneyDto(money.Amount, money.Currency);
        }

        public static Money ToDomain(this MoneyDto money)
        {
            ArgumentNullException.ThrowIfNull(money, nameof(money));

            return new Money(money.Amount, money.Currency);
        }
    }
}
