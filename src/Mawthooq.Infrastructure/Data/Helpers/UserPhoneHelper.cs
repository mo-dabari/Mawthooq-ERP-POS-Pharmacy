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
    internal static class UserPhoneHelper
    {
        public static List<Phone> GetPhonesByUserId(int userId, SqlConnection connection)
        {
            List<Phone> phones = new List<Phone>();

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "SELECT DialingCode, PhoneNumber, PhoneType FROM UserPhones WHERE UserId = @UserId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int dialingCode = reader.GetInt32(reader.GetOrdinal("DialingCode"));
                string phoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));

                if (!Enum.TryParse(reader.GetString(reader.GetOrdinal("PhoneType")), out PhoneType parsedPhoneType))
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

        public static void AddPhoneForUser(int userId, Phone phone, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "INSERT INTO UserPhones (UserId, CountryCode, DialingCode, PhoneNumber, PhoneType) VALUES (@UserId, @CountryCode, @DialingCode, @PhoneNumber, @PhoneType)";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@CountryCode", SqlDbType.Char, 3).Value = phone.CountryCode.ToString();
            command.Parameters.Add("@DialingCode", SqlDbType.VarChar, 5).Value = ((int)phone.CountryCode).ToString();
            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 15).Value = phone.Value;
            command.Parameters.Add("@PhoneType", SqlDbType.NVarChar).Value = phone.PhoneType.ToString();

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
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 15).Value = newPhone.Value;
            command.Parameters.Add("@OldPhoneNumber", SqlDbType.VarChar, 15).Value = oldPhone.Value;
            command.Parameters.Add("@CountryCode", SqlDbType.Char, 3).Value = newPhone.CountryCode.ToString();
            command.Parameters.Add("@DialingCode", SqlDbType.VarChar, 5).Value = ((int)newPhone.CountryCode).ToString();
            command.Parameters.Add("@PhoneType", SqlDbType.NVarChar).Value = newPhone.PhoneType.ToString();

            command.ExecuteNonQuery();
        }

        public static void DeletePhoneForUser(int userId, Phone phone, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "DELETE FROM UserPhones WHERE UserId = @UserId AND PhoneNumber = @PhoneNumber";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 15).Value = phone.Value;

            command.ExecuteNonQuery();
        }

        public static void DeletePhonesForUser(int userId, SqlConnection connection, SqlTransaction? transaction = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            string query = "DELETE FROM UserPhones WHERE UserId = @UserId";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            command.ExecuteNonQuery();
        }
    }
}
