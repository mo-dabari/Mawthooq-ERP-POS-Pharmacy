using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Customers;
using Mawthooq.Domain.ValueObjects;
using Mawthooq.Infrastructure.Data;
using Mawthooq.Infrastructure.Data.Helpers;
using Mawthooq.Infrastructure.Mappers;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Repositories
{
    public class CustomerRepositorySqlSever : ICustomerRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public CustomerRepositorySqlSever(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public Customer Add(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            string query = @"INSERT INTO Customers (FirstName, LastName)
                             VALUES (@FirstName, @LastName);
                             SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = (SqlConnection)_dbConnectionFactory.CreateConnection();

            SqlTransaction? transaction = null;
            connection.Open();

            try
            {
                transaction = connection.BeginTransaction();

                using var command = new SqlCommand(query, connection, transaction);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName).DbType = DbType.String;
                command.Parameters.AddWithValue("@LastName", customer.LastName).DbType = DbType.String;

                int newCustomerId = (int)command.ExecuteScalar();

                if (customer.contactNumbers != null)
                {
                    foreach (var phone in customer.contactNumbers)
                    {
                        CustomerPhoneHelper.AddPhoneForCustomer(newCustomerId, phone, connection, transaction);
                    }
                }
                transaction.Commit();

                return new Customer(newCustomerId, customer.FirstName, customer.LastName);
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }

        }

        public void Delete(int entity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entity, nameof(entity));

            string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";

            using var connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                CustomerPhoneHelper.DeletePhonesForCustomer(entity, connection, transaction);

                using var command = new SqlCommand(query, connection, transaction);
                command.Parameters.AddWithValue("@CustomerId", entity).DbType = DbType.Int32;

                command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            string query = @"SELECT
                                CustomerId, FirstName, LastName, IsActive, CreatedAt
                            FROM
                                Customers";

            using var connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();

            using var command = new SqlCommand(query, connection);

            using var reader = command.ExecuteReader();

            List<Customer> customers = new List<Customer>();

            while (reader.Read())
            {
                Customer customer = CustomerMapper.ToCustomerDomain(reader);

                reader.Close();

                customer.SetPhones(CustomerPhoneHelper.GetPhonesByCustomerId(customer.CustomerId, connection));

                customers.Add(customer);
            }

            return customers;
        }

        public Customer? GetById(int customerId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));

            string query = @"SELECT
                                CustomerId, FirstName, LastName, IsActive, CreatedAt
                            FROM
                                Customers
                            WHERE
                                CustomerId = @CustomerId";

            using SqlConnection connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerId", customerId).DbType = DbType.Int32;

            using SqlDataReader reader = command.ExecuteReader();

            Customer? user = null;

            if (reader.Read())
                user = reader.ToCustomerDomain();

            reader.Close();

            if (user != null)
            {
                user.SetPhones(CustomerPhoneHelper.GetPhonesByCustomerId(user.CustomerId, connection));
                return user;
            }

            return null;
        }

        public Customer? GetByPhoneNumber(string phone)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(phone, nameof(phone));

            string query = @"SELECT
                                CustomerID, FirstName, LastName, IsActive, CreatedAt
                         FROM
                            Customers C
                         Inner Join
                            CustomerPhones Cp
                        ON
                            C.CustomerId = Cp.CustomerId
                         And Cp.PhoneNumber = @Phone";

            using SqlConnection connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Phone", phone).DbType = DbType.String;
            using SqlDataReader reader = command.ExecuteReader();

            Customer? customer = null;

            if (reader.Read())
                customer = reader.ToCustomerDomain();

            reader.Close();

            if (customer != null)
            {
                customer.SetPhones(CustomerPhoneHelper.GetPhonesByCustomerId(customer.CustomerId, connection));
                return customer;
            }

            return null;
        }

        public void Update(Customer entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            string query = @"UPDATE Customers
                         SET FirstName = @FirstName, LastName = @LastName, IsActive = @IsActive
                         WHERE CustomerId = @CustomerId";

            using SqlConnection connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();


            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerId", entity.CustomerId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@FirstName", entity.FirstName).DbType = DbType.String;
            command.Parameters.AddWithValue("@LastName", entity.LastName).DbType = DbType.String;
            command.Parameters.AddWithValue("@IsActive", entity.IsActive).DbType = DbType.Boolean;

            command.ExecuteNonQuery();
        }

        public void UpdatePhone(int customerId, Phone oldPhone, Phone newPhone)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(oldPhone, nameof(oldPhone));
            ArgumentNullException.ThrowIfNull(newPhone, nameof(newPhone));

            using SqlConnection connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();

            CustomerPhoneHelper.UpdatePhoneForCustomer(customerId, oldPhone, newPhone, connection);
        }

        public void AddPhone(int customerId, Phone phone)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            using SqlConnection connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();

            CustomerPhoneHelper.AddPhoneForCustomer(customerId, phone, connection);
        }

        public void DeletePhone(int customerId, Phone phone)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            using SqlConnection connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();

            CustomerPhoneHelper.DeletePhoneForCustomer(customerId, phone, connection);
        }

        public void DeletePhones(int customerId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));

            using SqlConnection connection = (SqlConnection)_dbConnectionFactory.CreateConnection();
            connection.Open();

            CustomerPhoneHelper.DeletePhonesForCustomer(customerId, connection);
        }
    }
}
