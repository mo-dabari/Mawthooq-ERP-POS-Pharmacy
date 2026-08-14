using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a product's category is changed.
    /// </summary>
    public sealed record ProductCategoryChangedEvent(
        ProductId ProductId,
        CategoryId NewCategoryId,
        DateTime OccurredOn) : DomainEvent(OccurredOn);
}
