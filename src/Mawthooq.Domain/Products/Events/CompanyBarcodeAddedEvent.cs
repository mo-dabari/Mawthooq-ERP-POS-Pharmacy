using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.Events
{
    /// <summary>
    /// Raised when a company barcode is added to a unit.
    /// </summary>
    public sealed record CompanyBarcodeAddedEvent(
    ProductId ProductId,
    PackagingVersionId PackagingVersionId,
    UnitOfMeasure unit,
    Barcode barcode,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

}
