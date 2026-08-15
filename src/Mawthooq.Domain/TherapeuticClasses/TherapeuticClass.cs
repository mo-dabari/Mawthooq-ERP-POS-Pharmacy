using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Medicines.ValueObjects;
using Mawthooq.Domain.TherapeuticClasses.ValueObjects;

namespace Mawthooq.Domain.TherapeuticClasses
{
    public class TherapeuticClass
    {
        public int TherapeuticClassId { get; }
        public TherapeuticClassName TherapeuticClassName { get; private set; }

        public TherapeuticClass(int therapeuticClassId, TherapeuticClassName therapeuticClassName) : this(therapeuticClassName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(therapeuticClassId, nameof(therapeuticClassId));

            TherapeuticClassId = therapeuticClassId;
        }

        private TherapeuticClass(TherapeuticClassName therapeuticClassName)
        {
            ArgumentNullException.ThrowIfNull(therapeuticClassName,nameof(therapeuticClassName));

            TherapeuticClassName = therapeuticClassName;
        }

        public static TherapeuticClass Create(TherapeuticClassName therapeuticClassName)
        {
            return new TherapeuticClass(therapeuticClassName);
        }

        public void Rename(TherapeuticClassName newTherapeuticClassName)
        {
            ArgumentNullException.ThrowIfNull(newTherapeuticClassName , nameof(newTherapeuticClassName));

            if (TherapeuticClassName.Value.Equals(newTherapeuticClassName.Value, StringComparison.OrdinalIgnoreCase))
                return;

            TherapeuticClassName = newTherapeuticClassName;
        }
    }
}
