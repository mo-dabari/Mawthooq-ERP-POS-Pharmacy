using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Exceptions;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;
using Mawthooq.Infrastructure.AdoNet.Shared;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Repositories.Categories
{
    internal sealed class CategoryRepository : ICategoryRepository
    {
        private const string InsertSql = """
        INSERT INTO dbo.Categories (CategoryId, Name, Description, Status)
        OUTPUT INSERTED.RowVersion
        VALUES (@CategoryId, @Name, @Description, @Status);
        """;
        private const string UpdateSql = """
        UPDATE dbo.Categories
        SET Name = @Name, Description = @Description, Status = @Status
        OUTPUT INSERTED.RowVersion
        WHERE CategoryId = @CategoryId AND RowVersion = @ExpectedRowVersion;
        """;
        private const string SelectColumns = "SELECT CategoryId, Name, Description, Status, RowVersion FROM dbo.Categories";

        private readonly IDbConnectionProvider _connectionProvider;

        public CategoryRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider);
            _connectionProvider = connectionProvider;
        }

        public async Task<(Category? Entity, byte[]? RowVersion)> GetByIdAsync(CategoryId id, CancellationToken cancellationToken = default)
        {
            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);
            await using SqlCommand command = new($"{SelectColumns} WHERE CategoryId = @CategoryId;", connection);
            command.Parameters.Add("@CategoryId", SqlDbType.UniqueIdentifier).Value = id.Value;

            await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            return await reader.ReadAsync(cancellationToken) ? (Map(reader), reader.GetFieldValue<byte[]>(4)) : (null, null);
        }

        public async Task<bool> ExistsAndActiveAsync(CategoryId categoryId, CancellationToken cancellationToken = default)
        {
            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new("SELECT 1 FROM dbo.Categories WHERE CategoryId = @CategoryId AND Status = 1;", connection);

            command.Parameters.Add("@CategoryId", SqlDbType.UniqueIdentifier).Value = categoryId.Value;

            return await command.ExecuteScalarAsync(cancellationToken) is not null;
        }

        public async Task<(Category? Entity, byte[]? RowVersion)> AddAsync(Category entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(InsertSql, connection);

            AddParameters(command, entity);

            try
            {
                byte[] rowVersion = (byte[])(await command.ExecuteScalarAsync(cancellationToken))!;
                return (entity, rowVersion);
            }
            catch (SqlException ex) when (SqlErrors.IsUniqueViolation(ex, "UX_Categories_Name"))
            {
                throw new DuplicateCategoryNameException(entity.Name);
            }
        }

        public async Task<byte[]> UpdateAsync(Category entity, byte[] expectedRowVersion, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(expectedRowVersion, nameof(expectedRowVersion));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(UpdateSql, connection);

            AddParameters(command, entity);

            command.Parameters.Add("@ExpectedRowVersion", SqlDbType.Timestamp).Value = expectedRowVersion;

            try
            {
                object? result = await command.ExecuteScalarAsync(cancellationToken);

                return result is byte[] newRowVersion
                    ? newRowVersion
                    : throw new ConcurrencyConflictException(entity.CategoryId.Value);
            }
            catch (SqlException ex) when (SqlErrors.IsUniqueViolation(ex, "UX_Categories_Name"))
            {
                throw new DuplicateCategoryNameException(entity.Name);
            }
        }

        private static void AddParameters(SqlCommand command, Category entity)
        {
            command.Parameters.Add("@CategoryId", SqlDbType.UniqueIdentifier).Value = entity.CategoryId.Value;
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = entity.Name;
            command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = entity.Description;
            command.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)entity.Status;
        }

        private static Category Map(SqlDataReader reader)
        {
            Category category = Category.Create(CategoryId.From(reader.GetGuid(0)), reader.GetString(1), reader.GetString(2));

            if ((Status)reader.GetByte(3) == Status.Inactive)
                category.Deactivate();

            return category;
        }
    }
}
