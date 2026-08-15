using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Medicines.ValueObjects
{
    public sealed record MedicineId : DomainObjectId
    {
        private MedicineId(Guid Value) : base(Value)
        {}
        public static MedicineId New() => new(Guid.NewGuid());
        public static MedicineId From(Guid Value) => new(Value);
    }
}
