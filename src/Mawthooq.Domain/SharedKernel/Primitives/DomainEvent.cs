using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.SharedKernel.Primitives
{
    public abstract record DomainEvent(DateTime OccurredOn) : IDomainEvent;

}
