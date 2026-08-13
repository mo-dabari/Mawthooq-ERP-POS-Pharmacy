using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a new packaging version is added to a product.
    /// </summary>
    public sealed record PackagingVersionAddedEvent(
    ProductId ProductId,
    PackagingVersionId PackagingVersionId,
    int Version,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

}
