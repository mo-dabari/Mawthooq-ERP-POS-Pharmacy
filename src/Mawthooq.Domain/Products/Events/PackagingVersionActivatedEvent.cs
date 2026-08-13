using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a packaging version is activated.
    /// </summary>
    public sealed record PackagingVersionActivatedEvent(
    ProductId ProductId,
    PackagingVersionId PackagingVersionId,
    DateTime OccurredOn) : DomainEvent(OccurredOn);
}
