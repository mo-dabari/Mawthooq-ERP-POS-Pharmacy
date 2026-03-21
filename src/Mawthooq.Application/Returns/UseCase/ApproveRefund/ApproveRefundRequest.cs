using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Returns.UseCase.ApproveRefund
{
    public record class ApproveRefundRequest(int ReturnId)
    {
    }
}
