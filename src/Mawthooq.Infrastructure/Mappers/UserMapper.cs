using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.Users;
using Mawthooq.Domain.Users.Enums;
using Mawthooq.Domain.ValueObjects.NationalID;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Mappers
{
    public static class UserMapper
    {
        public static User ToUserDomain(this SqlDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            if (!Enum.TryParse(reader.GetString(reader.GetOrdinal("NationalId")).Substring(0, 2), out CountryCode Countrycode))
                throw new InvalidOperationException($"Invalid Country Code in National ID: {reader.GetString(reader.GetOrdinal("NationalId")).Substring(0, 2)}");

            return new User(
                reader.GetInt32(reader.GetOrdinal("UserId")),
                new NationalId(Countrycode, reader.GetString(reader.GetOrdinal("NationalId"))),
                reader.GetString(reader.GetOrdinal("FirstName")),
                reader.GetString(reader.GetOrdinal("MiddleName")),
                reader.GetString(reader.GetOrdinal("LastName")),
                DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DateOfBirth"))),
                (Role)reader.GetInt32(reader.GetOrdinal("RoleID"))
            );
        }
    }
}
