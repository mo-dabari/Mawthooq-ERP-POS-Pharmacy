using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Users;
using Mawthooq.Domain.Users.Enums;
using Mawthooq.Domain.ValueObjects;
using Mawthooq.Domain.ValueObjects.Enums;
using Mawthooq.Domain.ValueObjects.NationalID;
using Mawthooq.Infrastructure.Data;
using Mawthooq.Infrastructure.Data.Helpers;
using Mawthooq.Infrastructure.Mappers;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Repositories
{
    public class UserRepositorySqlServer : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepositorySqlServer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public User Add(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = @"INSERT INTO Users (NationalId, FirstName, MiddleName, LastName, DateOfBirth, RoleID)
                         VALUES (@NationalId, @FirstName, @MiddleName, @LastName, @DateOfBirth, @RoleID);
                         SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlTransaction? transaction = null;
            connection.Open();

            try
            {
                transaction = connection.BeginTransaction();

                using SqlCommand command = new SqlCommand(query, connection, transaction);
                command.Parameters.AddWithValue("@NationalId", user.NationalId.Value).DbType = DbType.String;
                command.Parameters.AddWithValue("@FirstName", user.FirstName).DbType = DbType.String;
                command.Parameters.AddWithValue("@MiddleName", user.MiddleName).DbType = DbType.String;
                command.Parameters.AddWithValue("@LastName", user.LastName).DbType = DbType.String;
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd")).DbType = DbType.Date;
                command.Parameters.AddWithValue("@RoleID", (int)user.Role).DbType = DbType.Int32;

                int newUserId = (int)command.ExecuteScalar();

                if (user.contactNumbers != null)
                {
                    foreach (var phone in user.contactNumbers)
                    {
                        UserPhoneHelper.AddPhoneForUser(newUserId, phone, connection, transaction);
                    }
                }
                transaction.Commit();

                return new User(newUserId, user.NationalId, user.FirstName, user.MiddleName, user.LastName, user.DateOfBirth, user.Role);
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
        }

        public void Delete(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = "DELETE FROM Users WHERE UserId = @UserId";

            SqlTransaction? transaction = null;
            connection.Open();

            try
            {
                transaction = connection.BeginTransaction();

                using SqlCommand command = new SqlCommand(query, connection, transaction);

                command.Parameters.AddWithValue("@UserId", userId).DbType = DbType.Int32;
                command.ExecuteNonQuery();

                UserPhoneHelper.DeletePhonesForUser(userId, connection, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }

        }

        public IEnumerable<User> GetAll()
        {
            string query = "SELECT UserId, NationalId, FirstName, MiddleName, LastName, DateOfBirth, RoleID FROM Users";

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();
            connection.Open();

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            var users = new List<User>();

            while (reader.Read())
            {
                users.Add(reader.ToUserDomain());
            }

            reader.Close();

            foreach (var user in users)
            {
                user.SetPhones(UserPhoneHelper.GetPhonesByUserId(user.UserId, connection));
            }
            return users;
        }

        public User? GetById(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));

            string query = "SELECT UserId, NationalId, FirstName, MiddleName, LastName, DateOfBirth, RoleID FROM Users WHERE UserId = @UserId";

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();
            connection.Open();

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId).DbType = DbType.Int32;

            using SqlDataReader reader = command.ExecuteReader();

            User? user = null;

            if (reader.Read())
                user = reader.ToUserDomain();

            reader.Close();

            if (user != null)
            {
                user.SetPhones(UserPhoneHelper.GetPhonesByUserId(user.UserId, connection));
                return user;
            }

            return null;
        }

        public User? GetByName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            string query = @"SELECT UserId, NationalId, FirstName, MiddleName, LastName, DateOfBirth, RoleID
                         FROM Users
                         WHERE CONCAT(FirstName, ' ', MiddleName, ' ', LastName) LIKE @Name";

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();
            connection.Open();

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", $"%{name}%").DbType = DbType.String;
            using SqlDataReader reader = command.ExecuteReader();

            User? user = null;

            if (reader.Read())
                user = reader.ToUserDomain();

            reader.Close();

            if (user != null)
            {
                user.SetPhones(UserPhoneHelper.GetPhonesByUserId(user.UserId, connection));
                return user;
            }

            return null;
        }

        public void Update(User entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            string query = @"UPDATE Users
                         SET NationalId = @NationalId, FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, DateOfBirth = @DateOfBirth, RoleID = @RoleID
                         WHERE UserId = @UserId";

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();
            connection.Open();


            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@NationalId", entity.NationalId.Value).DbType = DbType.String;
            command.Parameters.AddWithValue("@FirstName", entity.FirstName).DbType = DbType.String;
            command.Parameters.AddWithValue("@MiddleName", entity.MiddleName).DbType = DbType.String;
            command.Parameters.AddWithValue("@LastName", entity.LastName).DbType = DbType.String;
            command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth.ToString("yyyy-MM-dd")).DbType = DbType.Date;
            command.Parameters.AddWithValue("@RoleID", (int)entity.Role).DbType = DbType.Int32;

            command.ExecuteNonQuery();
        }

        public void UpdatePhone(int userId, Phone oldPhone, Phone newPhone)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(oldPhone, nameof(oldPhone));
            ArgumentNullException.ThrowIfNull(newPhone, nameof(newPhone));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();
            connection.Open();

            UserPhoneHelper.UpdatePhoneForUser(userId, oldPhone, newPhone, connection);
        }

        public void AddPhone(int userId, Phone phone)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();
            connection.Open();

            UserPhoneHelper.AddPhoneForUser(userId, phone, connection);
        }

        public void DeletePhone(int userId, Phone phone)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();
            connection.Open();

            UserPhoneHelper.DeletePhoneForUser(userId, phone, connection);
        }

        public void DeletePhones(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();
            connection.Open();

            UserPhoneHelper.DeletePhonesForUser(userId, connection);
        }

    }
}
