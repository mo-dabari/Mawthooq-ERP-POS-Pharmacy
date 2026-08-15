using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.TherapeuticClasses.ValueObjects
{
    public record class TherapeuticClassName
    {
        public string Value { get; }

        public TherapeuticClassName(string therapeuticClassName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(therapeuticClassName, nameof(therapeuticClassName));

            if (therapeuticClassName.Length > 100)
                throw new InvalidProgramException("Therapeutic Class name too long.");

            Value = therapeuticClassName.Trim();
        }
    }
}
