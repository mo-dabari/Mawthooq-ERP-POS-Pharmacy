using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.Events;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;

namespace Mawthooq.Infrastructure.AdoNet.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {

        private readonly IDbConnectionProvider _connectionProvider;

        private const string insertProductSql = """
                INSERT INTO dbo.Products
                    (ProductId, TradeName, Sku, CategoryId, ManufacturerId, LocalAgentId,
                     RequiresExpiryTracking, CanBeReturnedIfOpened, NearExpiryThresholdInDays,
                     Status, ImageStorageKey, ImageFileName, ImageMimeType)
                OUTPUT INSERTED.RowVersion
                VALUES
                    (@ProductId, @TradeName, @Sku, @CategoryId, @ManufacturerId, @LocalAgentId,
                     @RequiresExpiryTracking, @CanBeReturnedIfOpened, @NearExpiryThresholdInDays,
                     @Status, @ImageStorageKey, @ImageFileName, @ImageMimeType);
                """;

        private const string insertPackagingVersionSql = """
                INSERT INTO dbo.PackagingVersions (PackagingVersionId, ProductId, Version, Status)
                VALUES (@PackagingVersionId, @ProductId, @Version, @Status);
                """;

        private const string insertProductUnitSql = """
                INSERT INTO dbo.ProductUnits (PackagingVersionId, Unit, ConversionRate, Rank)
                OUTPUT INSERTED.ProductUnitId
                VALUES (@PackagingVersionId, @Unit, @ConversionRate, @Rank);
                """;

        private const string insertBarcodeSql = """
                INSERT INTO dbo.Barcodes (ProductUnitId, Value, Type)
                VALUES (@ProductUnitId, @Value, @Type);
                """;

        public ProductRepository(IDbConnectionProvider connectionProvider)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            _connectionProvider = connectionProvider;
        }

        public async Task<(Product? Entity, byte[]? RowVersion)> AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

            await using SqlCommand insertProductCommand = new(insertProductSql, connection, transaction);

            insertProductCommand.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = entity.ProductId.Value;
            insertProductCommand.Parameters.Add("@TradeName", SqlDbType.NVarChar, 100).Value = entity.TradeName;
            insertProductCommand.Parameters.Add("@Sku", SqlDbType.NVarChar, 50).Value = entity.SKU.Value;
            insertProductCommand.Parameters.Add("@CategoryId", SqlDbType.UniqueIdentifier).Value = entity.CategoryId.Value;
            insertProductCommand.Parameters.Add("@ManufacturerId", SqlDbType.UniqueIdentifier).Value = entity.ManufacturerId.Value;
            insertProductCommand.Parameters.Add("@LocalAgentId", SqlDbType.UniqueIdentifier).Value = entity.LocalAgentId.Value;
            insertProductCommand.Parameters.Add("@RequiresExpiryTracking", SqlDbType.Bit).Value = entity.RequiresExpiryTracking;
            insertProductCommand.Parameters.Add("@CanBeReturnedIfOpened", SqlDbType.Bit).Value = entity.CanBeReturnedIfOpened;
            insertProductCommand.Parameters.Add("@NearExpiryThresholdInDays", SqlDbType.Int).Value = entity.NearExpiryThresholdInDays;
            insertProductCommand.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)entity.Status;
            insertProductCommand.Parameters.Add("@ImageStorageKey", SqlDbType.NVarChar, 500).Value = entity.Image.StorageKey;
            insertProductCommand.Parameters.Add("@ImageFileName", SqlDbType.NVarChar, 100).Value = entity.Image.FileName;
            insertProductCommand.Parameters.Add("@ImageMimeType", SqlDbType.NVarChar, 50).Value = entity.Image.MimeType;

            object? rowVersionResult = await insertProductCommand.ExecuteScalarAsync(cancellationToken);

            if (rowVersionResult is not byte[] rowVersion)
                throw new InvalidOperationException("Failed to retrieve RowVersion after inserting the Product. Check the SQL OUTPUT clause.");

            foreach (PackagingVersion packagingVersion in entity.PackagingVersions)
            {
                await using SqlCommand insertVersionCommand = new(insertPackagingVersionSql, connection, transaction);
                insertVersionCommand.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = packagingVersion.PackagingVersionId.Value;
                insertVersionCommand.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = entity.ProductId.Value;
                insertVersionCommand.Parameters.Add("@Version", SqlDbType.Int).Value = packagingVersion.Version;
                insertVersionCommand.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)packagingVersion.Status;

                await insertVersionCommand.ExecuteNonQueryAsync(cancellationToken);

                foreach (ProductUnitDetails unit in packagingVersion.Units)
                {
                    await using SqlCommand insertUnitCommand = new(insertProductUnitSql, connection, transaction);
                    insertUnitCommand.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = packagingVersion.PackagingVersionId.Value;
                    insertUnitCommand.Parameters.Add("@Unit", SqlDbType.TinyInt).Value = (byte)unit.Unit;
                    insertUnitCommand.Parameters.Add("@ConversionRate", SqlDbType.Int).Value = unit.ConversionRate;
                    insertUnitCommand.Parameters.Add("@Rank", SqlDbType.TinyInt).Value = (byte)unit.Rank;

                    object? productUnitIdResult = await insertUnitCommand.ExecuteScalarAsync(cancellationToken);

                    if (productUnitIdResult is not int productUnitId)
                        throw new InvalidOperationException("Failed to retrieve ProductUnitId after inserting the ProductUnit. Check the SQL OUTPUT clause.");

                    foreach (Barcode barcode in unit.Barcodes)
                    {
                        await using SqlCommand insertBarcodeCommand = new(insertBarcodeSql, connection, transaction);
                        insertBarcodeCommand.Parameters.Add("@ProductUnitId", SqlDbType.Int).Value = productUnitId;
                        insertBarcodeCommand.Parameters.Add("@Value", SqlDbType.NVarChar, 50).Value = barcode.Value;
                        insertBarcodeCommand.Parameters.Add("@Type", SqlDbType.TinyInt).Value = (byte)barcode.Type;

                        await insertBarcodeCommand.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
            }
            await transaction.CommitAsync(cancellationToken);

            return (entity, rowVersion);
        }

        public async Task<bool> ExistsByBarcodeAsync(Barcode barcode, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(barcode, nameof(barcode));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            const string sql = """SELECT Top 1 1 FROM dbo.Barcodes WHERE Value = @Value""";

            await using SqlCommand command = new(sql, connection);
            command.Parameters.Add("@Value", SqlDbType.NVarChar, 50).Value = barcode.Value;

            object? result = await command.ExecuteScalarAsync(cancellationToken);

            return result is not null;
        }

        public async Task<bool> ExistsBySkuAsync(SKU sku, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(sku, nameof(sku));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            const string sql = """SELECT Top 1 1 FROM dbo.Products WHERE Sku = @Sku""";

            await using SqlCommand command = new(sql, connection);
            command.Parameters.Add("@Sku", SqlDbType.NVarChar, 50).Value = sku.Value;

            object? result = await command.ExecuteScalarAsync(cancellationToken);

            return result is not null;
        }

        public async Task<(Product? Entity, byte[]? RowVersion)> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            const string sql = """
        SELECT
            p.ProductId, p.TradeName, p.Sku, p.CategoryId, p.ManufacturerId, p.LocalAgentId,
            p.RequiresExpiryTracking, p.CanBeReturnedIfOpened, p.NearExpiryThresholdInDays,
            p.Status AS ProductStatus, p.ImageStorageKey, p.ImageFileName, p.ImageMimeType, p.RowVersion,
            pv.PackagingVersionId, pv.Version, pv.Status AS PackagingVersionStatus,
            pu.ProductUnitId, pu.Unit, pu.ConversionRate, pu.Rank,
            b.Value AS BarcodeValue, b.Type AS BarcodeType
        FROM dbo.Products p
        INNER JOIN dbo.PackagingVersions pv ON pv.ProductId = p.ProductId
        INNER JOIN dbo.ProductUnits pu ON pu.PackagingVersionId = pv.PackagingVersionId
        INNER JOIN dbo.Barcodes b ON b.ProductUnitId = pu.ProductUnitId
        WHERE p.ProductId = @ProductId;
        """;

            await using SqlCommand command = new(sql, connection);
            command.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = id.Value;

            await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

            List<ProductRow> rows = [];

            while (await reader.ReadAsync(cancellationToken))
            {
                rows.Add(ProductRow.FromReader(reader));
            }

            return rows.Count == 0 ? (null, null) : MapRowsToProduct(rows);
        }

        private static (Product Entity, byte[] RowVersion) MapRowsToProduct(IReadOnlyList<ProductRow> rows)
        {
            ArgumentNullException.ThrowIfNull(rows, nameof(rows));
            if (rows.Count == 0)
                throw new ArgumentException("Rows collection cannot be empty.", nameof(rows));

            ProductRow first = rows[0];
            IReadOnlyList<PackagingVersion> packagingVersions = MapPackagingVersions(rows);

            return (
                Product.Rehydrate(
                    ProductId.From(first.ProductId),
                    first.TradeName,
                    SKU.Create(first.Sku),
                    CategoryId.From(first.CategoryId),
                    ManufacturerId.From(first.ManufacturerId),
                    LocalAgentId.From(first.LocalAgentId),
                    (Status)first.ProductStatus,
                    first.RequiresExpiryTracking,
                    first.CanBeReturnedIfOpened,
                    first.NearExpiryThresholdInDays,
                    Image.Create(first.ImageStorageKey, first.ImageFileName, first.ImageMimeType),
                    packagingVersions),
                first.RowVersion);
        }

        private static IReadOnlyList<PackagingVersion> MapPackagingVersions(IEnumerable<ProductRow> rows)
        {
            ArgumentNullException.ThrowIfNull(rows, nameof(rows));

            return rows
                .GroupBy(pv => pv.PackagingVersionId)
                .Select(versionGroup =>
                    {
                        ProductRow versionRow = versionGroup.First();
                        IReadOnlyList<ProductUnitDetails> units = MapProductUnits(versionGroup);

                        return PackagingVersion.Rehydrate(
                            PackagingVersionId.From(versionGroup.Key),
                            versionRow.Version,
                            (Status)versionRow.PackagingVersionStatus,
                            units);
                    })
                .ToArray();
        }

        private static IReadOnlyList<ProductUnitDetails> MapProductUnits(IEnumerable<ProductRow> versionRows)
        {
            ArgumentNullException.ThrowIfNull(versionRows, nameof(versionRows));
            return versionRows
                .GroupBy(r => r.ProductUnitId)
                .Select(unitGroup =>
                {
                    ProductRow unitRow = unitGroup.First();
                    IReadOnlyList<Barcode> barcodes = MapBarcodes(unitGroup);

                    return ProductUnitDetails.Reconstruct(
                        (UnitOfMeasure)unitRow.Unit,
                        unitRow.ConversionRate,
                        (UnitRank)unitRow.Rank,
                        barcodes);
                })
                .ToArray();
        }

        private static IReadOnlyList<Barcode> MapBarcodes(IEnumerable<ProductRow> unitGroup)
        {
            ArgumentNullException.ThrowIfNull(unitGroup, nameof(unitGroup));

            return unitGroup
                .Select(br => UnitBarcodeFactory.Create(
                    br.BarcodeValue,
                    (BarcodeType)br.BarcodeType
                    ))
                .ToArray();
        }

        public async Task<byte[]> UpdateAsync(Product entity, byte[] expectedRowVersion, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(expectedRowVersion, nameof(expectedRowVersion));

            if (entity.DomainEvents.Count == 0)
                return expectedRowVersion;

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlTransaction transaction =
                (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

            bool productFieldsChanged = entity.DomainEvents.Any(e =>
                e is ProductRenamedEvent or
                ProductCategoryChangedEvent or
                ProductReturnPolicyChangedEvent or
                ProductStatusChangedEvent or
                ProductNearExpiryThresholdChangedEvent or
                ProductImageChangedEvent);

            byte[] newRowVersion;

            try
            {
                newRowVersion = productFieldsChanged
                    ? await UpdateProductFieldsAsync(entity, expectedRowVersion, connection, transaction, cancellationToken)
                    : await TouchProductRowVersionAsync(entity.ProductId, expectedRowVersion, connection, transaction, cancellationToken);

                foreach (var domainEvent in entity.DomainEvents)
                {
                    switch (domainEvent)
                    {
                        case PackagingVersionStatusEvent e:
                            await UpdatePackagingVersionStatusAsync(e, expectedRowVersion, connection, transaction, cancellationToken);
                            break;

                        case PackagingVersionAddedEvent e:
                            await InsertPackagingVersionAsync(entity, e.PackagingVersionId, connection, transaction, cancellationToken);
                            break;

                        case CompanyBarcodeAddedEvent e:
                            await InsertBarcodeAsync(e, connection, transaction, cancellationToken);
                            break;

                        case CompanyBarcodeRemovedEvent e:
                            await DeleteBarcodeAsync(e, connection, transaction, cancellationToken);
                            break;
                    }
                }
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            return newRowVersion;
        }

        private static async Task<byte[]> TouchProductRowVersionAsync(
            ProductId productId,
            byte[] expectedRowVersion,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            const string sql = """
                UPDATE dbo.Products
                SET ProductId = ProductId
                OUTPUT INSERTED.RowVersion
                WHERE ProductId = @ProductId AND RowVersion = @ExpectedRowVersion;
                """;

            await using SqlCommand command = new(sql, connection, transaction);

            command.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = productId.Value;
            command.Parameters.Add("@ExpectedRowVersion", SqlDbType.Timestamp).Value = expectedRowVersion;

            object? result = await command.ExecuteScalarAsync(cancellationToken);

            if (result is not byte[] newRowVersion)
                throw new InvalidOperationException(
                    $"Product {productId.Value} was modified by another user. Reload and try again.");

            return newRowVersion;
        }

        private static async Task<byte[]> UpdateProductFieldsAsync(
            Product entity,
            byte[] expectedRowVersion,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            const string sql = """
                UPDATE dbo.Products
                SET TradeName = @TradeName,
                    CategoryId = @CategoryId,
                    CanBeReturnedIfOpened = @CanBeReturnedIfOpened,
                    Status = @Status,
                    NearExpiryThresholdInDays = @NearExpiryThresholdInDays,
                    ImageStorageKey = @ImageStorageKey,
                    ImageFileName = @ImageFileName,
                    ImageMimeType = @ImageMimeType
                OUTPUT INSERTED.RowVersion
                WHERE ProductId = @ProductId AND RowVersion = @ExpectedRowVersion;
                """;

            await using SqlCommand command = new(sql, connection, transaction);

            command.Parameters.Add("@TradeName", SqlDbType.NVarChar, 100).Value = entity.TradeName;
            command.Parameters.Add("@CategoryId", SqlDbType.UniqueIdentifier).Value = entity.CategoryId.Value;
            command.Parameters.Add("@CanBeReturnedIfOpened", SqlDbType.Bit).Value = entity.CanBeReturnedIfOpened;
            command.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)entity.Status;
            command.Parameters.Add("@NearExpiryThresholdInDays", SqlDbType.Int).Value = entity.NearExpiryThresholdInDays;
            command.Parameters.Add("@ImageStorageKey", SqlDbType.NVarChar, 500).Value = entity.Image.StorageKey;
            command.Parameters.Add("@ImageFileName", SqlDbType.NVarChar, 100).Value = entity.Image.FileName;
            command.Parameters.Add("@ImageMimeType", SqlDbType.NVarChar, 50).Value = entity.Image.MimeType;
            command.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = entity.ProductId.Value;
            command.Parameters.Add("@ExpectedRowVersion", SqlDbType.Timestamp).Value = expectedRowVersion;

            object? result = await command.ExecuteScalarAsync(cancellationToken);

            if (result is not byte[] newRowVersion)
                throw new InvalidOperationException(
                    $"Product {entity.ProductId.Value} was modified by another user. Reload and try again.");

            return newRowVersion;
        }

        private static async Task UpdatePackagingVersionStatusAsync(
            PackagingVersionStatusEvent domainEvent,
            byte[] expectedRowVersion,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            const string sql = """
                UPDATE dbo.PackagingVersions
                SET Status = @Status
                WHERE PackagingVersionId = @PackagingVersionId
                AND RowVersion = @RowVersion;
                """;

            await using SqlCommand command = new(sql, connection, transaction);
            command.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)domainEvent.newStatus;
            command.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = domainEvent.PackagingVersionId.Value;
            command.Parameters.Add("@RowVersion", SqlDbType.Timestamp).Value = expectedRowVersion;

            int affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);

            if (affectedRows != 1)
                throw new InvalidOperationException(
                    $"PackagingVersion {domainEvent.PackagingVersionId.Value} was not found during status update.");
        }

        private static async Task InsertPackagingVersionAsync(
            Product entity,
            PackagingVersionId packagingVersionId,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            var packagingVersion = entity.PackagingVersions.Single(pv => pv.PackagingVersionId == packagingVersionId);

            const string insertVersionSql = """
                INSERT INTO dbo.PackagingVersions (PackagingVersionId, ProductId, Version, Status)
                VALUES (@PackagingVersionId, @ProductId, @Version, @Status);
                """;

            await using (SqlCommand versionCommand = new(insertVersionSql, connection, transaction))
            {
                versionCommand.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = packagingVersion.PackagingVersionId.Value;
                versionCommand.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = entity.ProductId.Value;
                versionCommand.Parameters.Add("@Version", SqlDbType.Int).Value = packagingVersion.Version;
                versionCommand.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)packagingVersion.Status;

                await versionCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            const string insertUnitSql = """
                INSERT INTO dbo.ProductUnits (PackagingVersionId, Unit, ConversionRate, Rank)
                OUTPUT INSERTED.ProductUnitId
                VALUES (@PackagingVersionId, @Unit, @ConversionRate, @Rank);
                """;

            const string insertBarcodeSql = """
                INSERT INTO dbo.Barcodes (ProductUnitId, Value, Type)
                VALUES (@ProductUnitId, @Value, @Type);
                """;

            foreach (var unit in packagingVersion.Units)
            {
                int productUnitId;

                await using (SqlCommand unitCommand = new(insertUnitSql, connection, transaction))
                {
                    unitCommand.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = packagingVersion.PackagingVersionId.Value;
                    unitCommand.Parameters.Add("@Unit", SqlDbType.TinyInt).Value = (byte)unit.Unit;
                    unitCommand.Parameters.Add("@ConversionRate", SqlDbType.Int).Value = unit.ConversionRate;
                    unitCommand.Parameters.Add("@Rank", SqlDbType.TinyInt).Value = (byte)unit.Rank;

                    object? result = await unitCommand.ExecuteScalarAsync(cancellationToken);

                    if (result is not int id)
                        throw new InvalidOperationException("Failed to retrieve ProductUnitId after inserting the ProductUnit.");

                    productUnitId = id;
                }

                foreach (var barcode in unit.Barcodes)
                {
                    await using SqlCommand barcodeCommand = new(insertBarcodeSql, connection, transaction);
                    barcodeCommand.Parameters.Add("@ProductUnitId", SqlDbType.Int).Value = productUnitId;
                    barcodeCommand.Parameters.Add("@Value", SqlDbType.NVarChar, 50).Value = barcode.Value;
                    barcodeCommand.Parameters.Add("@Type", SqlDbType.TinyInt).Value = (byte)barcode.Type;

                    await barcodeCommand.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        private static async Task InsertBarcodeAsync(
            CompanyBarcodeAddedEvent domainEvent,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            const string findUnitSql = """
                SELECT pu.ProductUnitId
                FROM dbo.ProductUnits pu
                WHERE pu.PackagingVersionId = @PackagingVersionId AND pu.Unit = @Unit;
                """;

            int productUnitId;

            await using (SqlCommand findCommand = new(findUnitSql, connection, transaction))
            {
                findCommand.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = domainEvent.PackagingVersionId.Value;
                findCommand.Parameters.Add("@Unit", SqlDbType.TinyInt).Value = (byte)domainEvent.unit;

                object? result = await findCommand.ExecuteScalarAsync(cancellationToken);

                if (result is not int id)
                    throw new InvalidOperationException(
                        $"ProductUnit not found for PackagingVersionId {domainEvent.PackagingVersionId.Value} and Unit {domainEvent.unit}.");

                productUnitId = id;
            }

            const string insertBarcodeSql = """
                INSERT INTO dbo.Barcodes (ProductUnitId, Value, Type)
                VALUES (@ProductUnitId, @Value, @Type);
                """;

            await using SqlCommand insertCommand = new(insertBarcodeSql, connection, transaction);
            insertCommand.Parameters.Add("@ProductUnitId", SqlDbType.Int).Value = productUnitId;
            insertCommand.Parameters.Add("@Value", SqlDbType.NVarChar, 50).Value = domainEvent.barcode.Value;
            insertCommand.Parameters.Add("@Type", SqlDbType.TinyInt).Value = (byte)domainEvent.barcode.Type;

            await insertCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        private static async Task DeleteBarcodeAsync(
            CompanyBarcodeRemovedEvent domainEvent,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            const string sql = """
                DELETE b
                FROM dbo.Barcodes b
                INNER JOIN dbo.ProductUnits pu ON pu.ProductUnitId = b.ProductUnitId
                WHERE pu.PackagingVersionId = @PackagingVersionId
                  AND pu.Unit = @Unit
                  AND b.Value = @Value;
                """;

            await using SqlCommand command = new(sql, connection, transaction);
            command.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = domainEvent.PackagingVersionId.Value;
            command.Parameters.Add("@Unit", SqlDbType.TinyInt).Value = (byte)domainEvent.unit;
            command.Parameters.Add("@Value", SqlDbType.NVarChar, 50).Value = domainEvent.barcode.Value;

            int affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);

            if (affectedRows != 1)
                throw new InvalidOperationException(
                    $"Barcode '{domainEvent.barcode.Value}' was not found for deletion under PackagingVersionId {domainEvent.PackagingVersionId.Value}.");
        }
    }
}
