using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Medicines.Enums;

namespace Mawthooq.Domain.Medicines.ValueObjects
{
    public record class DosageForm
    {
        public DosageFormType Value { get; }
        public DosageForm(DosageFormType dosageFormType)
        {
            if (!Enum.IsDefined(typeof(DosageFormType), dosageFormType))
                throw new InvalidEnumArgumentException(nameof(dosageFormType), (int)dosageFormType, typeof(DosageFormType));

                Value = dosageFormType;
        }
    }
}
