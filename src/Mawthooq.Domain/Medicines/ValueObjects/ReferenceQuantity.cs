using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Medicines.Enums;

namespace Mawthooq.Domain.Medicines.ValueObjects
{
    public class ReferenceQuantity
    {
        public decimal Value { get; }
        public ReferenceQuantityUnits Unit { get; }

        public ReferenceQuantity(decimal referenceQuantity, ReferenceQuantityUnits unit)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(referenceQuantity, nameof(referenceQuantity));

            if (!Enum.IsDefined(typeof(ReferenceQuantityUnits), unit))
                throw new InvalidEnumArgumentException(nameof(unit), (int)unit, typeof(ReferenceQuantityUnits));

            if (unit == ReferenceQuantityUnits.capsule || unit == ReferenceQuantityUnits.tablet)
                referenceQuantity = Math.Round(referenceQuantity);

            Value = referenceQuantity;
            Unit = unit;
        }

            public override string ToString() => $"{Value} {Unit}";
    }
}
