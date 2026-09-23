using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;

namespace Mawthooq.Application.Products.Exceptions
{
    public sealed class DuplicateSkuException : ConflictException
    {
        public DuplicateSkuException(string sku)
            : base($"SKU '{sku}' already exists.")
        {}
    }
}
