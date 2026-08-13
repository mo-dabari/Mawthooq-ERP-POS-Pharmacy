using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.Enums;

namespace Mawthooq.Domain.Inventories.Entities
{
    public class AdjustmentLog
    {
        public int AdjustmentId { get; }
        public int QuantityBefore { get; }
        public int QuantityAfter { get; }
        public int Difference { get; }
        public AdjustmentReason Reason { get; }
        public DateTime AdjustedAt { get; }
        public AdjustmentLog(int adjustmentId,int quantityBefore, int quantityAfter, AdjustmentReason reason)
        : this(quantityBefore, quantityAfter, reason)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(adjustmentId, nameof(adjustmentId));
            AdjustmentId = adjustmentId;
        }

        private AdjustmentLog(int quantityBefore, int quantityAfter, AdjustmentReason reason)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(quantityBefore, nameof(quantityBefore));
            ArgumentOutOfRangeException.ThrowIfNegative(quantityAfter, nameof(quantityAfter));

            if (!Enum.IsDefined(reason))
                throw new InvalidEnumArgumentException(nameof(reason), (int)reason, typeof(AdjustmentReason));

            QuantityBefore = quantityBefore;
            QuantityAfter = quantityAfter;
            Difference = quantityAfter - quantityBefore;
            Reason = reason;
            AdjustedAt = DateTime.UtcNow;
        }

        public static AdjustmentLog Create(int quantityBefore, int quantityAfter, AdjustmentReason reason)
        {
            return new AdjustmentLog(quantityBefore, quantityAfter, reason);
        }
    }
}
