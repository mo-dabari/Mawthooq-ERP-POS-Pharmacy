using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Users.Enums;
using Mawthooq.Domain.ValueObjects.NationalID;
using Microsoft.Data.SqlClient;
using Mawthooq.Domain.Customers;

namespace Mawthooq.Infrastructure.Mappers
{
    public static class CustomerMapper
    {
        public static Customer ToCustomerDomain(this SqlDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            Customer customer = new Customer(
                reader.GetInt32(reader.GetOrdinal("CustomerID")),
                reader.GetString(reader.GetOrdinal("FirstName")),
                reader.GetString(reader.GetOrdinal("LastName"))
            );

            if (!reader.GetBoolean(reader.GetOrdinal("IsActive")))
                customer.Deactivate();

                customer.SetCreatedAt(reader.GetDateTime(reader.GetOrdinal("CreatedAt")));

            return customer;
        }
    }
}
