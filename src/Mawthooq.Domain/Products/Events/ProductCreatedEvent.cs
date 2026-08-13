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
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Products.Events
{
/// <summary>
/// Raised when a new product is created.
/// Contains the complete initial state of the product.
/// </summary>
public sealed record ProductCreatedEvent(
    ProductId ProductId,
    string TradeName,
    SKU Sku,
    CategoryId CategoryId,
    ManufacturerId ManufacturerId,
    LocalAgentId LocalAgentId,
    bool RequiresExpiryTracking,
    bool CanBeReturnedIfOpened,
    int NearExpiryThresholdInDays,
    Image Image,
    DateTime OccurredOn
) : DomainEvent(OccurredOn);
}
