using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;

namespace Mawthooq.Application.Manufacturers.Exceptions
{
    public sealed class ManufacturerNotFoundException : NotFoundException
    {
        public ManufacturerNotFoundException(Guid manufacturerId)
            : base($"Manufacturer with id '{manufacturerId}' was not found or is inactive.") { }
    }
}
