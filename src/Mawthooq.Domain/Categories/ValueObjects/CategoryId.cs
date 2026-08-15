using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Categories.ValueObjects
{
    public sealed record CategoryId : DomainObjectId
    {
        private CategoryId(Guid value) : base(value)
        {
        }
        public static CategoryId New() => new(Guid.NewGuid());
        public static CategoryId From(Guid guid) => new(guid);
    }
}

