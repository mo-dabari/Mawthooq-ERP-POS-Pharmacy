using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.ValueObjects
{
    public sealed record ProductId : DomainObjectId
    {
        private ProductId(Guid value) : base(value)
        {
        }
        public static ProductId New() => new(Guid.NewGuid());
        public static ProductId From(Guid value) => new(value);
    }
}
