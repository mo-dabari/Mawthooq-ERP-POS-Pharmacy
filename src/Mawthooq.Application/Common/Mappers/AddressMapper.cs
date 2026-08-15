using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Application.Common.DTOs.Mappers
{
    public static class AddressMapper
    {
        public static AddressDto ToAddressDto(this Address address)
        {
            ArgumentNullException.ThrowIfNull(address, nameof(address));

            return new AddressDto(address.BuildingNumber, address.Street, address.City, address.Governorate, address.Country);
        }

        public static Address ToAddressDomain(this AddressDto addressDto)
        {
            ArgumentNullException.ThrowIfNull(addressDto, nameof(addressDto));

            return new Address(addressDto.BuildingNumber, addressDto.Street, addressDto.City, addressDto.Governorate, addressDto.Country);
        }
    }
}
