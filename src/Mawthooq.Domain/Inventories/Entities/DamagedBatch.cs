using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories.Enums;

namespace Mawthooq.Domain.Inventories.Entities
{
    public class DamagedBatch
    {
        public int DamagedBatchId { get; private set; }
        public int Quantity { get; }
        public DamagedReason Reason { get; }
        public DateTime DamagedAt { get; }

        public DamagedBatch(int damagedBatchId, int quantity, DamagedReason reason)
        : this(quantity, reason)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damagedBatchId, nameof(damagedBatchId));
            DamagedBatchId = damagedBatchId;
        }

        private DamagedBatch(int quantity, DamagedReason reason)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));

            if (!Enum.IsDefined(reason))
                throw new InvalidEnumArgumentException(nameof(reason), (int)reason, typeof(DamagedReason));

            Quantity = quantity;
            Reason = reason;
            DamagedAt = DateTime.UtcNow;
        }

        public static DamagedBatch Create(int quantity, DamagedReason reason)
        {
            return new DamagedBatch(quantity, reason);
        }

        public void SetDamagedBatchId(int damagedBatchId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damagedBatchId, nameof(damagedBatchId));
            DamagedBatchId = damagedBatchId;
        }
    }
}
