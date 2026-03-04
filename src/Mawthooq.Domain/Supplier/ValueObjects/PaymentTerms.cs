using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Supplier.ValueObjects
{
    public record class PaymentTerms
    {
        public short NetDays { get; }
        public short DiscountDays { get; }
        public decimal DiscountPercentage { get; }
        public decimal PenaltyPercentage { get; }

        public PaymentTerms(short netDays, short discountDays, decimal discountPercentage, decimal pdenaltyPercentage)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(netDays);

            NetDays = netDays;

            if (netDays > 0)
            {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(discountDays);
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(discountPercentage);
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pdenaltyPercentage);

                if (discountDays >= netDays)
                    throw new ArgumentException("Discount days must be less than net days.");

                DiscountDays = discountDays;
                DiscountPercentage = discountPercentage;
                PenaltyPercentage = pdenaltyPercentage;

            }
            else
            {
                DiscountDays = 0;
                DiscountPercentage = 0;
                PenaltyPercentage = 0;
            }
        }

        public DateTime CalculateDueDate(DateTime invoiceDate)
        {
            if (invoiceDate > DateTime.UtcNow)
                throw new InvalidOperationException("Invalid Invoice Date");

            if (NetDays == 0)
                return invoiceDate;

            else
                return invoiceDate.AddDays(NetDays);
        }
    }
}
