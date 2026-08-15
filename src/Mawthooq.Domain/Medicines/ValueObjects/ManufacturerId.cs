using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Medicines.ValueObjects
{
    public record class ManufacturerId
    {
        public int Value { get; }

        public ManufacturerId(int manufacturerId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(manufacturerId , nameof(manufacturerId));

            Value = manufacturerId;
        }
    }
}
