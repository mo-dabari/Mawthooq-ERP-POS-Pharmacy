using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Payments
{
    public interface IPaymentRepository
    {
        Payment? Add(Payment payment);
        Payment? GetBySaleId(int saleId);
    }
}
