using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.Primitives
{
    public record DomainObjectId
    {
        public Guid Value { get; }
        protected DomainObjectId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("the identifier (GUID) cannot be empty");

            Value = value;
        }
    }
}
