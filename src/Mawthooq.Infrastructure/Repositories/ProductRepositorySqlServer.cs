using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
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
                return new Product(newProductId, product.Name, product.SKU, product.CategoryId, product.Packaging, product.RequiresExpiryTracking, product.CanBeReturnedIfOpened, product.NearExpiryThresholdInDays);

            throw new InvalidOperationException("Failed to insert Product.");
        }
        public Product? GetById(int productId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId);

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = @"Select
                                ProductId, Name, SKU, Barcode, ImageUrl, CategoryId,
                                p.Unitbase, RequiresExpiryTracking, CanBeReturnedIfOpened, NearExpiryThresholdInDays, Status
                            From
                                Product
                            Inner Join
                                ProductPackaging p
                            on
                                Product.ProductId = p.ProductId
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
            command.Parameters.AddWithValue("@IsActive", product.Status.ToString()).DbType = DbType.String;

            connection.Open();

            command.ExecuteNonQuery();
        }
        public bool ExistsBySku(SKU sku)
        {
            ArgumentNullException.ThrowIfNull(sku, nameof(sku));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = @"Select Count(1) From Product Where SKU = @SKU";

            using SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@SKU", sku).DbType = DbType.String;

            connection.Open();

            int count = (int)command.ExecuteScalar();

            return count > 0;
        }
        public bool ExistsByBarcode(Barcode barcode)
        {
            ArgumentNullException.ThrowIfNull(barcode, nameof(barcode));

            using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

            string query = @"Select Count(1) From Product Where Barcode = @Barcode";

            using SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@Barcode", barcode).DbType = DbType.String;

            connection.Open();

            int count = (int)command.ExecuteScalar();

            return count > 0;
        }
    }
}
