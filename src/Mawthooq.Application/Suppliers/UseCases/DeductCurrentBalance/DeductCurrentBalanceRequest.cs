using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs;

namespace Mawthooq.Application.Suppliers.UseCases.DeductCurrentBalance
{
    public record class DeductCurrentBalanceRequest(int SupplierId, MoneyDto Balance)
    {

    }
}
