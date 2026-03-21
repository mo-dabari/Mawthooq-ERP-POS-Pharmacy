using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Sales.Entity;

namespace Mawthooq.Application.Sales.UseCases.RemoveSaleLine
{
    public record class RemoveSaleLineRequest(int SaleId, int SaleLineId)
    {

    }
}
