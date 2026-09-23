using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Domain.Products.ValueObjects;

namespace Mawthooq.Application.Products.Exceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid productId)
            : base($"Product with ID '{productId}' not found.")
        {
        }
    }
}
