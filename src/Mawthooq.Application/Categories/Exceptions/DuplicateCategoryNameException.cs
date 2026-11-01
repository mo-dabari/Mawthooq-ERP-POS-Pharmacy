using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;

namespace Mawthooq.Application.Categories.Exceptions
{
    public sealed class DuplicateCategoryNameException : ConflictException
    {
        public DuplicateCategoryNameException(string categoryName)
            : base($"Categoty Name '{categoryName}' already exists.")
        { }
    }
}
