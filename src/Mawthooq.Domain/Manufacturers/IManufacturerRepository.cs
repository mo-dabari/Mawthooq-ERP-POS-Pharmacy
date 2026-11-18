using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Manufacturers.ValueObjects;

namespace Mawthooq.Domain.Manufacturers
{
    public interface IManufacturerRepository
    {
        Task<bool> ExistsAndActiveAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken = default);
    }
}
