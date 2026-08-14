using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Products.ValueObjects
{
    public record class PackagingVersionId :DomainObjectId
    {
        private PackagingVersionId(Guid value) : base(value)
        {
        }

        public static PackagingVersionId New()
            => new(Guid.NewGuid());

        public static PackagingVersionId From(Guid value)
            => new(value);
    }
}
