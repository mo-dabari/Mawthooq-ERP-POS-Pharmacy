using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.ValueObjects;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Data.Helpers
{
    internal static class CustomerPhoneHelper
    {
        internal static List<Phone> GetPhonesByCustomerId(int customerId, SqlConnection connection)
        {
            List<Phone> phones = new List<Phone>();

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "SELECT DialingCode, PhoneNumber, PhoneType FROM CustomerPhones WHERE CustomerId = @CustomerId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int dialingCode = reader.GetInt32(reader.GetOrdinal("DialingCode"));
                string phoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));

                if(!Enum.TryParse(reader.GetString(reader.GetOrdinal("PhoneType")), out PhoneType parsedPhoneType))
                    throw new Exception($"Invalid PhoneType value: {reader.GetString(reader.GetOrdinal("PhoneType"))}");

                PhoneType phoneType = parsedPhoneType;

                if (!Enum.TryParse(dialingCode.ToString(), out CountryCode countryCode))
                    throw new Exception($"Invalid DialingCode: {dialingCode}");

                if (phoneNumber.StartsWith("0"))
                    phoneNumber = phoneNumber.Substring(1);

                phones.Add(new Phone(countryCode, phoneNumber, phoneType));
            }
            return phones;
        }

        internal static void AddPhoneForCustomer(int customerId, Phone phone, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "INSERT INTO CustomerPhones (CustomerId, CountryCode, DialingCode, PhoneNumber, PhoneType) VALUES (@CustomerId, @CountryCode, @DialingCode, @PhoneNumber, @PhoneType)";

            using SqlCommand command = new SqlCommand(query, connection, transaction);


            command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@CountryCode", SqlDbType.Char, 3).Value = phone.CountryCode.ToString();
            command.Parameters.Add("@DialingCode", SqlDbType.VarChar, 5).Value = ((int)phone.CountryCode).ToString();
            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 15).Value = phone.Value;
            command.Parameters.Add("@PhoneType", SqlDbType.VarChar).Value = phone.PhoneType.ToString();

            command.ExecuteNonQuery();
        }

        internal static void AddPhonesForCustomer(int customerId, IEnumerable<Phone> phones, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(phones, nameof(phones));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            foreach (var phone in phones)
            {
                AddPhoneForCustomer(customerId, phone, connection, transaction);
            }
        }

        internal static void UpdatePhoneForCustomer(int customerId, Phone oldPhone, Phone newPhone, SqlConnection connection, SqlTransaction? transaction = null)
        {

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(oldPhone, nameof(oldPhone));
            ArgumentNullException.ThrowIfNull(newPhone, nameof(newPhone));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = @"UPDATE
                                CustomerPhones
                            SET
                                CountryCode = @CountryCode, DialingCode = @DialingCode, PhoneNumber = @PhoneNumber , PhoneType = @PhoneType
                            WHERE
                                CustomerId = @CustomerId
                            AND
                               PhoneNumber = @OldPhoneNumber";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 15).Value = newPhone.Value;
            command.Parameters.Add("@OldPhoneNumber", SqlDbType.VarChar, 15).Value = oldPhone.Value;
            command.Parameters.Add("@CountryCode", SqlDbType.Char, 3).Value = newPhone.CountryCode.ToString();
            command.Parameters.Add("@DialingCode", SqlDbType.VarChar, 5).Value = ((int)newPhone.CountryCode).ToString();
            command.Parameters.Add("@PhoneType", SqlDbType.VarChar).Value = newPhone.PhoneType.ToString();

            command.ExecuteNonQuery();
        }

        internal static void DeletePhoneForCustomer(int customerId, Phone phone, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "DELETE FROM CustomerPhones WHERE CustomerId = @CustomerId AND PhoneNumber = @PhoneNumber";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 15).Value = phone.Value;

            command.ExecuteNonQuery();
        }

        internal static void DeletePhonesForCustomer(int customerId, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "DELETE FROM CustomerPhones WHERE CustomerId = @CustomerId";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

            command.ExecuteNonQuery();
        }
    }
}
