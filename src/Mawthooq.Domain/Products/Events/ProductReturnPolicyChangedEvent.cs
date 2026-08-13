using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a product's "can be returned if opened" policy changes.
    /// </summary>
    public sealed record ProductReturnPolicyChangedEvent(
        ProductId ProductId,
        bool CanBeReturnedIfOpened,
        DateTime OccurredOn) : DomainEvent(OccurredOn);
}
