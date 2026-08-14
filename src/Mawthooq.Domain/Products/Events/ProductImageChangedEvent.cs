using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Primitives;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a product's image is changed.
    /// </summary>
    public sealed record ProductImageChangedEvent(
        ProductId ProductId,
        Image Image,
        DateTime OccurredOn) : DomainEvent(OccurredOn);
}
