using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Returns;

namespace Mawthooq.Application.Returns.UseCase.RejectRefund
{
    public class RejectRefundUseCase
    {
        private readonly IReturnRepository _returnRepository;
        public RejectRefundUseCase(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }

        public RejectRefundResponse Execute(RejectRefundRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var returnEntity = _returnRepository.GetReturnById(request.ReturnId);
            if (returnEntity == null)
                throw new InvalidOperationException("Return request not found.");

            if (!returnEntity.Refunds.Any())
                throw new InvalidOperationException("No refund associated with this return request.");

            foreach (var refund in returnEntity.Refunds)
                refund.Reject();

            _returnRepository.Update(returnEntity);
            return new RejectRefundResponse(true);
        }
    }
}
