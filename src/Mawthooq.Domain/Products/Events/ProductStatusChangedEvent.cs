using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a product's status changes (Active/Inactive).
    /// </summary>
    public sealed record ProductStatusChangedEvent(
        ProductId ProductId,
        Status Status,
        DateTime OccurredOn) : DomainEvent(OccurredOn);
}
