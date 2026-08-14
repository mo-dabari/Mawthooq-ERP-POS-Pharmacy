using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a product's trade name is changed.
    /// </summary>
    public sealed record ProductRenamedEvent(
        ProductId ProductId,
        string NewTradeName,
        DateTime OccurredOn) : DomainEvent(OccurredOn);
}
