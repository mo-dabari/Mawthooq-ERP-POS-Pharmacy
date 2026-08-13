using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;

namespace Mawthooq.Domain.Products.ValueObjects.Packaging
{
    public abstract record class ProductPackaging
    {
        public PackagingTemplate Template { get; }
        internal ProductPackaging(PackagingTemplate template)
        {
            if (!Enum.IsDefined(typeof(PackagingTemplate), template))
                throw new InvalidEnumArgumentException(nameof(template), (int)template, typeof(PackagingTemplate));

            Template = template;
        }

        internal abstract IEnumerable<ProductUnitDefinition> GetUnitDefinitions();
    }
}
