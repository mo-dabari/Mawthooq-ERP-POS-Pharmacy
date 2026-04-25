using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Products;
using Mawthooq.Infrastructure.Data;
using Mawthooq.Infrastructure.Mappers;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Repositories
{
    public class ProductRepositorySqlServer : IProductRepository
    {

        private readonly IDbConnectionFactory _connectionFactory;

        public ProductRepositorySqlServer(IDbConnectionFactory connectionFactory)
        {
            ArgumentNullException.ThrowIfNull(connectionFactory, nameof(connectionFactory));
            _connectionFactory = connectionFactory;
        }

        public Product Add(Product product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = @"Insert
                            Into
                                Product
                                    (Name, SKU, Barcode, ImageUrl, CategoryId)
                            Values
                                (@Name, @SKU, @Barcode, @ImageUrl, @CategoryId)
                                Select Cast(SCOPE_IDENTITY() as Int)";

            SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@Name", product.Name).DbType = DbType.String;
            command.Parameters.AddWithValue("@SKU", product.SKU).DbType = DbType.String;
            command.Parameters.AddWithValue("@Barcode", product.Barcode).DbType = DbType.String;
            command.Parameters.AddWithValue("@ImageUrl", product.ImageUrl).DbType = DbType.String;
            command.Parameters.AddWithValue("@CategoryId", product.CategoryId).DbType = DbType.Int32;

            connection.Open();
            object result = command.ExecuteScalar();

            if (int.TryParse(result.ToString(), out int newProductId))
                return new(newProductId, product.Name, product.SKU, product.Barcode, product.ImageUrl, product.CategoryId);


            throw new InvalidOperationException("Failed to insert Product.");
        }

        public void Delete(int productId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = @"Delete From Product WHERE ProductId = @productId";

            using SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@productId", productId).DbType = DbType.Int32;

            connection.Open();

            command.ExecuteNonQuery();
        }

        public Product? GetById(int productId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId);

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = @"Select
                                Name, SKU, Barcode, ImageUrl, CategoryId, IsActive
                            From
                                Product
                            Where
                                ProductId = @productId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@productId", productId).DbType = DbType.Int32;

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
                return reader.ToProductDomain();

            return default;

        }

        public Product? GetProductByName(string ProductName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProductsIsActive()
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = @"Update
                                Product
                            Set
                                CategoryId = @CategoryId, Name = @Name, ImageUrl = @ImageUrl , SKU = @SKU, Barcode = @Barcode, IsActive = @IsActive
                            Where
                                ProductId = @productId";

            using SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@productId", product.ProductId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@CategoryId", product.CategoryId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@Name", product.Name).DbType = DbType.String;
            command.Parameters.AddWithValue("@ImageUrl", product.ImageUrl).DbType = DbType.String;
            command.Parameters.AddWithValue("@SKU", product.SKU).DbType = DbType.String;
            command.Parameters.AddWithValue("@Barcode", product.Barcode).DbType = DbType.String;
            command.Parameters.AddWithValue("@IsActive", product.IsActive).DbType = DbType.Boolean;

            connection.Open();

            command.ExecuteNonQuery();
        }
    }
}
