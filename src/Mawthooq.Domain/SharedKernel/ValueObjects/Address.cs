using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mawthooq.Domain.SharedKernel.ValueObjects
{
    public class Address
    {
        public int BuildingNumber { get; private set; }
        public string Street { get; private set; }
        public string Neighborhood { get; private set; }
        public string Governorate { get; private set; }
        public string Country { get; private set; }

        public Address(int buildingNumber, string street, string neighborhood, string governorate, string country)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(buildingNumber, nameof(buildingNumber));

            ArgumentException.ThrowIfNullOrWhiteSpace(street, nameof(street));
            ArgumentException.ThrowIfNullOrWhiteSpace(neighborhood, nameof(neighborhood));
            ArgumentException.ThrowIfNullOrWhiteSpace(governorate, nameof(governorate));
            ArgumentException.ThrowIfNullOrWhiteSpace(country, nameof(country));

            BuildingNumber = buildingNumber;
            Street = street.Trim();
            Neighborhood = neighborhood.Trim();
            Governorate = governorate.Trim();
            Country = country.Trim();
        }
        public override string ToString()
        {
            return $"{BuildingNumber}-{Street} St-{Neighborhood}-{Governorate}-{Country}";
        }

        public bool Equals(Address? address)
        {
            return
                BuildingNumber == address?.BuildingNumber &&
                Street == address.Street &&
                Neighborhood == address.Neighborhood &&
                Governorate == address.Governorate &&
                Country == address.Country;
        }

        public override bool Equals(object? obj)
        {
            return obj is Address other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BuildingNumber, Street, Neighborhood, Governorate, Country);
        }
    }
}
