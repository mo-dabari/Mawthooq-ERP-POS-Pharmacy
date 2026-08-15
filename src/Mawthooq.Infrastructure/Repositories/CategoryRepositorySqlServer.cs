using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;
using Mawthooq.Infrastructure.Data;
using Mawthooq.Infrastructure.Mappers;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Repositories
{
    public class CategoryRepositorySqlServer : ICategoryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CategoryRepositorySqlServer(IDbConnectionFactory connectionFactory)
        {
            ArgumentNullException.ThrowIfNull(connectionFactory, nameof(connectionFactory));
            _connectionFactory = connectionFactory;
        }
        public Category Add(Category category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            using var connection = _connectionFactory.CreateConnection();

            string query = @"Insert
                                Into
                                    Category (Name, Description)
                            Values
                                (@categoryName , @categoryDescription);
                            SELECT
                                CAST(SCOPE_IDENTITY() as int);";

            using SqlCommand command = new SqlCommand(query, (SqlConnection)connection);

            command.Parameters.AddWithValue("@categoryName", category.Name).DbType = DbType.String;
            command.Parameters.AddWithValue("@categoryDescription", category.Description).DbType = DbType.String;

            connection.Open();

            object result = command.ExecuteScalar();

            if (int.TryParse(result.ToString(), out int newCategoryID))
            {
                Category newCategory = Category.Create(category.Name, category.Description);
                // newCategory.SetCategoryId(newCategoryID);
                return newCategory;
            }
            throw new InvalidOperationException("Failed to insert category.");
        }

        public void Delete(int categoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(categoryId, nameof(categoryId));

            using var connection = _connectionFactory.CreateConnection();

            string query = @"Delete From
                                Category
                            WHERE
                                CategoryID = @CategoryID;";

            using SqlCommand command = new SqlCommand(query, (SqlConnection)connection);

            command.Parameters.AddWithValue("@CategoryID", categoryId).DbType = DbType.Int32;

            connection.Open();

            command.ExecuteNonQuery();
        }

        public bool ExistsAndActive(int categoryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Category? GetById(int categoryID)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryID, nameof(categoryID));

            using var connection = _connectionFactory.CreateConnection();

            string query = @"Select
                                CategoryID, Name, Description,IsActive
                            From
                                Category
                            Where
                                CategoryID = @categoryID";

            using var command = connection.CreateCommand();
            command.CommandText = query;

            var parmeter = command.CreateParameter();

            parmeter.ParameterName = "@categoryID";
            parmeter.Value = categoryID;
            parmeter.DbType = DbType.Int32;
            command.Parameters.Add(parmeter);

            if (connection.State != ConnectionState.Open)
                ((SqlConnection)connection).Open();

            connection.Open();
            using var reader = ((SqlCommand)command).ExecuteReader();

            if (reader.Read())
                return reader.ToCategoryDomain();

            return null;
        }

        public Category? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Update(Category category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            using var connection = _connectionFactory.CreateConnection();

            string query = @"Update
                                Category
                            Set
                                Name = @categoryName, Description = @categoryDescription, IsActive = @IsActive
                            WHERE
                                CategoryID = @categoryID;";

            using SqlCommand command = new SqlCommand(query, (SqlConnection)connection);

            command.Parameters.AddWithValue("@categoryName", category.Name).DbType = DbType.String;
            command.Parameters.AddWithValue("@categoryDescription", category.Description).DbType = DbType.String;
            command.Parameters.AddWithValue("@categoryID", category.CategoryId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@IsActive", category.IsActive).DbType = DbType.Boolean;

            connection.Open();

            command.ExecuteNonQuery();
        }
    }
}
