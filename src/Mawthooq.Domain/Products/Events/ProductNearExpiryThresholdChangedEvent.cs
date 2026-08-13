using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a product's near-expiry threshold (in days) changes.
    /// </summary>
    public sealed record ProductNearExpiryThresholdChangedEvent(
        ProductId ProductId,
        int NearExpiryThresholdInDays,
        DateTime OccurredOn) : DomainEvent(OccurredOn);
}
