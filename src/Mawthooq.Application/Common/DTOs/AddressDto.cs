using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Common.DTOs
{
    public record class AddressDto(
        int BuildingNumber,
        string Street,
        string City,
        string Governorate,
        string Country);
}
