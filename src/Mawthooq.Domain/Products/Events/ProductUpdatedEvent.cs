using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when any product properties are updated.
    /// This is a coarse-grained event for business corrections.
    /// </summary>
    public record ProductUpdatedEvent(
        ProductId ProductId,
        string TradeName,
        CategoryId CategoryId,
        ManufacturerId ManufacturerId,
        LocalAgentId LocalAgentId,
        string ImageName,
        bool CanBeReturnedIfOpened,
        int NearExpiryThresholdInDays,
        Status Status,
        DateTime OccurredOn
    ) : DomainEvent(OccurredOn);
}
