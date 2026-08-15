using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.Abstractions
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
