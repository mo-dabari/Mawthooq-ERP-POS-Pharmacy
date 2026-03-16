using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Inventories.UseCases.ReportDamagedStock
{
    public record class ReportDamagedResponse(
        int DamagedBatchId,
        int Quantity,
        string Reason,
        DateTime DamagedAt
    )
    {

    }
}
