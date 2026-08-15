using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.Entities;
using Mawthooq.Domain.Inventories.ValueObjects;
using Mawthooq.Infrastructure.Data;
using Mawthooq.Infrastructure.Mappers;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Repositories
{
    public class InventoryRepositorySqlServer : IInventoryRepository
    {
        private readonly IDbConnectionFactory _dbConnection;

        public InventoryRepositorySqlServer(IDbConnectionFactory dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Inventory Add(Inventory inventory)
        {
            ArgumentNullException.ThrowIfNull(inventory, nameof(inventory));

            string query = @"INSERT INTO Inventory (ProductId, BranchId, Quantity , SellingPrice, CurrencyCode)
                             VALUES (@ProductId, @BranchId, @Quantity, @SellingPrice, @CurrencyCode);
                             SELECT CAST(SCOPE_IDENTITY() as int)";

            using SqlConnection sqlConnection = (SqlConnection)_dbConnection.CreateConnection();

            SqlTransaction? transaction = null;

            try
            {
                sqlConnection.Open();

                transaction = sqlConnection.BeginTransaction();

                using SqlCommand command = new(query, sqlConnection, transaction);

                command.Parameters.Add("@ProductId", SqlDbType.Int).Value = inventory.ProductId;
                command.Parameters.Add("@BranchId", SqlDbType.Int).Value = inventory.BranchId;
                command.Parameters.Add("@Quantity", SqlDbType.Int).Value = inventory.GetAvailableQuantity();
                SqlParameter price = command.Parameters.Add("@SellingPrice", SqlDbType.Decimal);
                price.Precision = 18;
                price.Scale = 2;
                price.Value = inventory.SellingPrice.Amount;
                command.Parameters.Add("@CurrencyCode", SqlDbType.VarChar, 3).Value = inventory.SellingPrice.Currency.ToString();

                command.Parameters.AddWithValue("@ProductId", inventory.ProductId).SqlDbType = SqlDbType.Int;
                command.Parameters.AddWithValue("@BranchId", inventory.BranchId).DbType = DbType.Int32;
                command.Parameters.AddWithValue("@Quantity", inventory.GetAvailableQuantity()).DbType = DbType.Int32;
                command.Parameters.AddWithValue("@SellingPrice", inventory.SellingPrice.Amount).SqlDbType = SqlDbType.Decimal;
                command.Parameters.AddWithValue("@CurrencyCode", inventory.SellingPrice.Currency.ToString()).DbType = DbType.String;

                int newInventoryId = (int)command.ExecuteScalar();

                if (newInventoryId <= 0)
                    throw new InvalidOperationException("Failed to insert inventory and retrieve its ID.");

                InsertBatches(newInventoryId, inventory.Batches, sqlConnection, transaction);

                transaction.Commit();
                return inventory;
            }
            catch (Exception)
            {
                transaction?.Rollback();
                throw;
            }
        }

        public Inventory? GetByInventoryKey(InventoryKey inventoryKey)
        {
            ArgumentNullException.ThrowIfNull(inventoryKey, nameof(inventoryKey));

            string query = @"SELECT InventoryId, ProductId, BranchId, SellingPrice, CurrencyCode
                             FROM Inventory
                             WHERE ProductId = @ProductId AND BranchId = @BranchId";

            using SqlConnection sqlConnection = (SqlConnection)_dbConnection.CreateConnection();

            SqlCommand command = new(query, sqlConnection);
            command.Parameters.AddWithValue("@ProductId", inventoryKey.ProductId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@BranchId", inventoryKey.BranchId).DbType = DbType.Int32;

            sqlConnection.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                throw new InvalidOperationException($"Inventory with ProductId {inventoryKey.ProductId} and BranchId {inventoryKey.BranchId} not found.");

            int inventoryId = reader.GetInt32(reader.GetOrdinal("InventoryId"));
            IEnumerable<Batch> batches = GetBatchesByInventoryId(inventoryId, sqlConnection);
            Inventory inventory = InventoryMapper.MapToInventory(reader, batches);
            reader.Close();
            return inventory;
        }

        public void Update(Inventory inventory)
        {
            ArgumentNullException.ThrowIfNull(inventory, nameof(inventory));

            string query = @"UPDATE Inventory
                             SET SellingPrice = @SellingPrice, CurrencyCode = @CurrencyCode
                             WHERE ProductId = @ProductId AND BranchId = @BranchId";

            using SqlConnection sqlConnection = (SqlConnection)_dbConnection.CreateConnection();

            sqlConnection.Open();

            using SqlCommand command = new(query, sqlConnection);
            command.Parameters.AddWithValue("@ProductId", inventory.ProductId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@BranchId", inventory.BranchId).DbType = DbType.Int32;
            command.Parameters.AddWithValue("@SellingPrice", inventory.SellingPrice.Amount).DbType = DbType.Decimal;
            command.Parameters.AddWithValue("@CurrencyCode", inventory.SellingPrice.Currency.ToString()).DbType = DbType.String;

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
                throw new InvalidOperationException($"Inventory with ProductId {inventory.ProductId} and BranchId {inventory.BranchId} not found.");
        }

        public Inventory UpdateAndReturn(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        private void InsertBatch(int inventoryId, Batch batch, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inventoryId, nameof(inventoryId));
            ArgumentNullException.ThrowIfNull(batch, nameof(batch));

            string batchQuery = @"INSERT INTO Batches (InventoryId, Quantity, PurchaseCost, ReceivedDate, ExpiryDate)
                                  VALUES (@InventoryId, @Quantity, @PurchaseCost, @ReceivedDate, @ExpiryDate);
                                  SELECT CAST(SCOPE_IDENTITY() as int)";

            using SqlCommand batchCommand = new(batchQuery, sqlConnection, transaction);
            batchCommand.Parameters.AddWithValue("@InventoryId", inventoryId).DbType = DbType.Int32;
            batchCommand.Parameters.AddWithValue("@Quantity", batch.Quantity).DbType = DbType.Int32;
            batchCommand.Parameters.AddWithValue("@PurchaseCost", batch.PurchaseCost.Amount).DbType = DbType.Decimal;
            batchCommand.Parameters.AddWithValue("@ReceivedDate", batch.ReceivedDate).DbType = DbType.DateTime;
            batchCommand.Parameters.AddWithValue("@ExpiryDate", batch.ExpiryDate).DbType = DbType.Date;

            int newBatchId = (int)batchCommand.ExecuteScalar();
            if (newBatchId <= 0)
                    throw new InvalidOperationException("Failed to insert batch and retrieve its ID.");
        }
        private void InsertBatches(int inventoryId, IEnumerable<Batch> batches, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inventoryId, nameof(inventoryId));
            ArgumentNullException.ThrowIfNull(batches, nameof(batches));

            foreach (Batch batch in batches)
            {
                InsertBatch(inventoryId, batch, sqlConnection, transaction);
            }
        }
        private void UpdateBatch(int inventoryId, Batch batch, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inventoryId, nameof(inventoryId));
            ArgumentNullException.ThrowIfNull(batch, nameof(batch));
            ArgumentNullException.ThrowIfNull(sqlConnection, nameof(sqlConnection));
            ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));

            string batchQuery = @"UPDATE Batches
                                    SET
                                        Quantity = @Quantity, PurchaseCost = @PurchaseCost,
                                        ReceivedDate = @ReceivedDate, ExpiryDate = @ExpiryDate
                                    WHERE
                                        BatchId = @BatchId
                                    AND InventoryId = @InventoryId";

            using SqlCommand batchCommand = new(batchQuery, sqlConnection, transaction);
            batchCommand.Parameters.AddWithValue("@BatchId", batch.BatchId).DbType = DbType.Int32;
            batchCommand.Parameters.AddWithValue("@InventoryId", inventoryId).DbType = DbType.Int32;
            batchCommand.Parameters.AddWithValue("@Quantity", batch.Quantity).DbType = DbType.Int32;
            batchCommand.Parameters.AddWithValue("@PurchaseCost", batch.PurchaseCost.Amount).DbType = DbType.Decimal;
            batchCommand.Parameters.AddWithValue("@ReceivedDate", batch.ReceivedDate).DbType = DbType.DateTime;
            batchCommand.Parameters.AddWithValue("@ExpiryDate", batch.ExpiryDate).DbType = DbType.Date;

            int rowsAffected = batchCommand.ExecuteNonQuery();
            if (rowsAffected == 0)
                throw new InvalidOperationException($"Batch with ID {batch.BatchId} not found for Inventory ID {inventoryId}.");
        }
        private List<Batch> GetBatchesByInventoryId(int inventoryId, SqlConnection connection)
        {
            List<Batch> batches = new();

            string query = @"SELECT
                                BatchId, Quantity, PurchaseCost, CurrencyCode, ReceivedDate, ExpiryDate
                             FROM
                                Batches
                             WHERE
                                InventoryId = @InventoryId
                             ORDER BY
                                ExpiryDate ASC, ReceivedDate ASC";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@InventoryId", inventoryId).DbType = DbType.Int32;


            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
                batches.Add(BatchMapper.MapToBatch(reader));

            return batches;
        }

        public IEnumerable<Inventory> Get_All_Available_Stock_For_Each_Branch()
        {
            throw new NotImplementedException();
        }

        public Inventory? GetById(InventoryKey id)
        {
            throw new NotImplementedException();
        }
    }
}
