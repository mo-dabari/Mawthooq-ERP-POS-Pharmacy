using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Suppliers.UseCases.IncreaseCurrentBalance
{
    public record class IncreaseCurrentBalanceResponse(int supplierId, MoneyDto CurrentBalance)
    {

    }
}
