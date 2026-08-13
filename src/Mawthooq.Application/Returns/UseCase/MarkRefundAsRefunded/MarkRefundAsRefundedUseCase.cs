using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Returns;
using Mawthooq.Domain.Returns.Entity;

namespace Mawthooq.Application.Returns.UseCase.MarkRefundAsRefunded
{
    public class MarkRefundAsRefundedUseCase
    {
         private readonly IReturnRepository _returnRepository;
        public MarkRefundAsRefundedUseCase(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }

        public MarkRefundAsRefundedResponse Execute(MarkRefundAsRefundedRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Return? returnToApprove = _returnRepository.GetReturnById(request.ReturnId);
            if (returnToApprove is null)
                throw new InvalidOperationException($"Return with id {request.ReturnId} not found.");

            foreach (Refund refund in returnToApprove.Refunds)
                refund.MarkAsRefunded();

                _returnRepository.Update(returnToApprove);

            return new MarkRefundAsRefundedResponse(true);
        }
    }
}
