using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mawthooq.Domain.ValueObjects
{
    public class Address
    {
        public int BuildingNumber { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string Governorate { get; private set; }
        public string Country { get; private set; }

        public Address(int buildingNumber, string street, string city, string governorate, string country)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(buildingNumber, nameof(buildingNumber));

            ArgumentException.ThrowIfNullOrWhiteSpace(street, nameof(street));

            ArgumentException.ThrowIfNullOrWhiteSpace(city, nameof(city));
            ArgumentException.ThrowIfNullOrWhiteSpace(governorate, nameof(governorate));
            ArgumentException.ThrowIfNullOrWhiteSpace(country, nameof(country));

            BuildingNumber = buildingNumber;
            Street = street;
            City = city;
            Governorate = governorate;
            Country = country;
        }
        public override string ToString()
        {
            return $"{BuildingNumber}-{Street}-{City}-{Governorate}-{Country}";
        }

        public bool Equals(Address address)
        {
            ArgumentNullException.ThrowIfNull(address, nameof(address));

            return
                BuildingNumber == address.BuildingNumber &&
                Street == address.Street &&
                City == address.City &&
                Governorate == address.Governorate &&
                Country == address.Country;
        }

        public override bool Equals(object? obj)
        {
            return obj is Address other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BuildingNumber, Street, City, Governorate, Country);
        }
    }
}
