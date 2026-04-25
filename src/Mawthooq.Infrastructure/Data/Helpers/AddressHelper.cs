using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Data.Helpers
{
    internal static class AddressHelper
    {
        internal static int InsertAddress(Address address, SqlConnection connection, SqlTransaction transaction)
        {

            string queryAddress = @"Insert
                             Into
                                 Address (BuildingNumber, Street, City, Governorate, Country)
                             Values
                                 (@BuildingNumber, @Street, @City, @Governorate, @Country);
                             Select SCOPE_IDENTITY();";

            var (cityId, governorateId, countryId) = GetLocationIds(address, connection, transaction);

            using SqlCommand command = new SqlCommand(queryAddress, connection, transaction);

            command.Parameters.AddWithValue("@BuildingNumber", address.BuildingNumber);
            command.Parameters.AddWithValue("@Street", address.Street);
            command.Parameters.AddWithValue("@City", cityId);
            command.Parameters.AddWithValue("@Governorate", governorateId);
            command.Parameters.AddWithValue("@Country", countryId);

            int addressId = Convert.ToInt32(command.ExecuteScalar());
            return addressId;

        }

        internal static Address GetAddressById(int addressId, SqlConnection connection)
        {
            string query = @"Select
                                BuildingNumber,
                                Street,
                                Ci.Name as City,
                                G.Name as Governorate,
                                Co.Name as Country
                            From
                                Address as A
                            Join
                                City as Ci on a.City = Ci.CityId
                            Join
                                Governorate as G on A.Governorate = G.GovernorateId
                            Join
                                Country as Co on A.Country = Co.CountryId
                            Where
                                AddressId = @AddressId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AddressId", addressId).DbType = DbType.Int32;

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int buildingNumber = reader.GetInt32(reader.GetOrdinal("BuildingNumber"));
                string street = reader.GetString(reader.GetOrdinal("Street"));
                string city = reader.GetString(reader.GetOrdinal("City"));
                string governorate = reader.GetString(reader.GetOrdinal("Governorate"));
                string country = reader.GetString(reader.GetOrdinal("Country"));

                return new Address(buildingNumber, street, city, governorate, country);
            }
            throw new InvalidOperationException("Address not found.");
        }

        internal static void UpdateAddress(int addressId, Address? newAddress, SqlConnection connection, SqlTransaction transaction)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(addressId, nameof(addressId));
            ArgumentNullException.ThrowIfNull(newAddress, nameof(newAddress));
            string query = @"Update
                                Address
                            Set
                                BuildingNumber = @BuildingNumber,
                                Street = @Street,
                                City = @City,
                                Governorate = @Governorate,
                                Country = @Country
                            Where
                                AddressId = @AddressId";

            var (cityId, governorateId, countryId) = GetLocationIds(newAddress, connection, transaction);

            using SqlCommand command = new SqlCommand(query, connection, transaction);

            command.Parameters.AddWithValue("@BuildingNumber", newAddress?.BuildingNumber).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@Street", newAddress?.Street).DbType = DbType.String;
            command.Parameters.AddWithValue("@City", cityId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@Governorate", governorateId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@Country", countryId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@AddressId", addressId).DbType = DbType.Int32;

            command.ExecuteNonQuery();
        }

        internal static string FormatAddress(Address address)
        {
            return address.ToString();
        }

        internal static int GetCountryId(string? countryName, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"Select CountryId From Country Where Name = @CountryName";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@CountryName", countryName).DbType = DbType.String;
            return Convert.ToInt32(command.ExecuteScalar());
        }

        internal static int GetCityId(string? cityName, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"Select CityId From City Where Name = @CityName";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@CityName", cityName).DbType = DbType.String;

            return Convert.ToInt32(command.ExecuteScalar());
        }

        internal static int GetGovernorateId(string? governorateName, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"Select GovernorateId From Governorate Where Name = @GovernorateName";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@GovernorateName", governorateName).DbType = DbType.String;

            return Convert.ToInt32(command.ExecuteScalar());
        }

        private static (int CityId, int GovernorateId, int CountryId) GetLocationIds
            (Address? address, SqlConnection connection, SqlTransaction transaction)
        {
            int cityId = GetCityId(address?.City, connection, transaction);
            int governorateId = GetGovernorateId(address?.Governorate, connection, transaction);
            int countryId = GetCountryId(address?.Country, connection, transaction);
            return (cityId, governorateId, countryId);
        }
    }
}
