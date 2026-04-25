using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects;
using Mawthooq.Domain.ValueObjects.Enums;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Data.Helpers
{
    internal static class UserPhoneHelper
    {
        public static List<Phone> GetPhonesByUserId(int userId, SqlConnection connection)
        {
            List<Phone> phones = new List<Phone>();

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "SELECT DialingCode, PhoneNumber, PhoneType FROM UserPhones WHERE UserId = @UserId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId).DbType = DbType.Int32;

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int dialingCode = reader.GetInt32(reader.GetOrdinal("DialingCode"));
                string phoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));
                PhoneType phoneType = (PhoneType)reader.GetInt32(reader.GetOrdinal("PhoneType"));

                if (!Enum.IsDefined(typeof(CountryCode), dialingCode))
                    throw new Exception($"Invalid DialingCode: {dialingCode}");

                if (phoneNumber.StartsWith("0"))
                    phoneNumber = phoneNumber.Substring(1);

                CountryCode countryCode = (CountryCode)dialingCode;
                phones.Add(new Phone(countryCode, phoneNumber, phoneType));
            }
            return phones;
        }

        public static void AddPhoneForUser(int userId, Phone phone, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "INSERT INTO UserPhones (UserId, CountryCode, DialingCode, PhoneNumber, PhoneType) VALUES (@UserId, @CountryCode, @DialingCode, @PhoneNumber, @PhoneType)";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@UserId", userId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@CountryCode", phone.CountryCode.ToString()).DbType = DbType.String;
            command.Parameters.AddWithValue("@DialingCode", ((int)phone.CountryCode).ToString()).DbType = DbType.String;
            command.Parameters.AddWithValue("@PhoneNumber", phone.Value).DbType = DbType.String;
            command.Parameters.AddWithValue("@PhoneType", (int)phone.PhoneType).DbType = DbType.Int32;

            command.ExecuteNonQuery();
        }

        public static void AddPhonesForUser(int userId, IEnumerable<Phone> phones, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(phones, nameof(phones));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            foreach (var phone in phones)
            {
                AddPhoneForUser(userId, phone, connection, transaction);
            }
        }

        public static void UpdatePhoneForUser(int userId, Phone oldPhone, Phone newPhone, SqlConnection connection, SqlTransaction? transaction = null)
        {

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(oldPhone, nameof(oldPhone));
            ArgumentNullException.ThrowIfNull(newPhone, nameof(newPhone));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = @"UPDATE
                                UserPhones
                            SET
                                CountryCode = @CountryCode, DialingCode = @DialingCode, PhoneNumber = @PhoneNumber , PhoneType = @PhoneType
                            WHERE
                                UserId = @UserId
                            AND
                               PhoneNumber = @OldPhoneNumber";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@UserId", userId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@PhoneNumber", newPhone.Value).DbType = DbType.String;
            command.Parameters.AddWithValue("@OldPhoneNumber", oldPhone.Value).DbType = DbType.String;
            command.Parameters.AddWithValue("@CountryCode", newPhone.CountryCode.ToString()).DbType = DbType.String;
            command.Parameters.AddWithValue("@DialingCode", ((int)newPhone.CountryCode).ToString()).DbType = DbType.String;
            command.Parameters.AddWithValue("@PhoneType", (int)newPhone.PhoneType).DbType = DbType.Int32;

            command.ExecuteNonQuery();
        }

        public static void DeletePhoneForUser(int userId, Phone phone, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "DELETE FROM UserPhones WHERE UserId = @UserId AND PhoneNumber = @PhoneNumber";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@UserId", userId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@PhoneNumber", phone.Value).DbType = DbType.String;

            command.ExecuteNonQuery();
        }

        public static void DeletePhonesForUser(int userId, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "DELETE FROM UserPhones WHERE UserId = @UserId";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@UserId", userId).DbType = DbType.Int32;

            command.ExecuteNonQuery();
        }
    }
}
