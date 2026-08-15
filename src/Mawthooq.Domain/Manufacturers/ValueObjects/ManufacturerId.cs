using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Primitives;

namespace Mawthooq.Domain.Manufacturers.ValueObjects
{
    public sealed record ManufacturerId : DomainObjectId
    {
        private ManufacturerId(Guid value) : base(value)
        {
        }
        public static ManufacturerId New() => new(Guid.NewGuid());
        public static ManufacturerId From(Guid value) => new(value);
    }
}
