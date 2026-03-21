using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Common.Interfaces;

namespace Mawthooq.Domain.Payments
{
    public interface IPaymentRepository
    {
        Payment? Add(Payment payment);
        Payment? GetBySaleId(int saleId);
    }
}
