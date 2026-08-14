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
    /// Raised when a packaging version  status changes (Active/Inactive).
    /// </summary>
    public sealed record PackagingVersionStatusEvent(
    ProductId ProductId,
    PackagingVersionId PackagingVersionId,
    Status newStatus,
    DateTime OccurredOn) : DomainEvent(OccurredOn);
}
