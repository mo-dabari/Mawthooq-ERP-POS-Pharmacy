using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.Exceptions;
using Mawthooq.Application.Products.Exceptions;
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
using Mawthooq.Infrastructure.AdoNet.Shared;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.AdoNet.Repositories.Products
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        private const string GetByIdSql = """
            SELECT
                p.ProductId, p.TradeName, p.Sku, p.CategoryId, p.ManufacturerId, p.LocalAgentId,
                p.RequiresExpiryTracking, p.CanBeReturnedIfOpened, p.NearExpiryThresholdInDays,
                p.Status AS ProductStatus, p.ImageStorageKey, p.ImageFileName, p.ImageMimeType, p.RowVersion,
                pv.PackagingVersionId, pv.Version, pv.Status AS PackagingVersionStatus,
                pu.ProductUnitId, pu.Unit, pu.ConversionRate, pu.Rank,
                b.Value AS BarcodeValue, b.Type AS BarcodeType

            FROM dbo.Products p

            INNER JOIN
                dbo.PackagingVersions pv
            ON
                pv.ProductId = p.ProductId

            INNER JOIN
                dbo.ProductUnits pu
            ON
                pu.PackagingVersionId = pv.PackagingVersionId

            INNER JOIN
                dbo.Barcodes b
            ON
                b.ProductUnitId = pu.ProductUnitId

            WHERE
                p.ProductId = @ProductId

            ORDER BY
                pv.Version, pu.Rank, b.BarcodeId;
        """;

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

        private const string UpdatePackagingVersionStatusSql = """
            UPDATE
                dbo.PackagingVersions
            SET
                Status = @Status
            WHERE
                PackagingVersionId = @PackagingVersionId;
            """;

        private const string UpdateProductFieldsSql = """
            UPDATE
                dbo.Products
            SET
                TradeName = @TradeName,
                CategoryId = @CategoryId,
                CanBeReturnedIfOpened = @CanBeReturnedIfOpened,
                Status = @Status,
                NearExpiryThresholdInDays = @NearExpiryThresholdInDays,
                ImageStorageKey = @ImageStorageKey,
                ImageFileName = @ImageFileName,
                ImageMimeType = @ImageMimeType

            OUTPUT
                INSERTED.RowVersion

            WHERE
                ProductId = @ProductId
            AND
                RowVersion = @ExpectedRowVersion;
        """;

        private const string TouchProductRowVersionSql = """
            UPDATE
                dbo.Products
            SET
                Status = Status
            OUTPUT
                INSERTED.RowVersion
            WHERE
                ProductId = @ProductId
            AND
                RowVersion = @ExpectedRowVersion;
        """;

        private const string DeleteBarcodeSql = """
            DELETE
                b
            FROM
                dbo.Barcodes b

            INNER JOIN
                dbo.ProductUnits pu
            ON
                pu.ProductUnitId = b.ProductUnitId

            WHERE
                pu.PackagingVersionId = @PackagingVersionId
            AND
                pu.Unit = @Unit
            AND
                b.Value = @Value;
            """;

        private const string FindProductUnitIdSql = """
            SELECT
                pu.ProductUnitId
            FROM
                dbo.ProductUnits pu
            WHERE
                pu.PackagingVersionId = @PackagingVersionId
            AND
                pu.Unit = @Unit;
        """;

        private const string ExistsBySkuSql = """
            SELECT
                Top 1 1
            FROM
                dbo.Products
            WHERE
                Sku = @Sku;
        """;

        private const string ExistsByBarcodeSql = """
            SELECT
                Top 1 1
            FROM
                dbo.Barcodes
            WHERE
                Value = @Value;
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

            byte[] rowVersion = await InsertProductAsync(entity, connection, transaction, cancellationToken);

            foreach (PackagingVersion packagingVersion in entity.PackagingVersions)
            {
                await InsertPackagingVersionGraphAsync(entity.ProductId.Value, packagingVersion, connection, transaction, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            entity.ClearDomainEvents();

            return (entity, rowVersion);
        }

        private static async Task<byte[]> InsertProductAsync(Product entity, SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken)
        {
            await using SqlCommand command = new(insertProductSql, connection, transaction);

            command.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = entity.ProductId.Value;
            command.Parameters.Add("@TradeName", SqlDbType.NVarChar, 100).Value = entity.TradeName;
            command.Parameters.Add("@Sku", SqlDbType.NVarChar, 50).Value = entity.SKU.Value;
            command.Parameters.Add("@CategoryId", SqlDbType.UniqueIdentifier).Value = entity.CategoryId.Value;
            command.Parameters.Add("@ManufacturerId", SqlDbType.UniqueIdentifier).Value = entity.ManufacturerId.Value;
            command.Parameters.Add("@LocalAgentId", SqlDbType.UniqueIdentifier).Value = entity.LocalAgentId.Value;
            command.Parameters.Add("@RequiresExpiryTracking", SqlDbType.Bit).Value = entity.RequiresExpiryTracking;
            command.Parameters.Add("@CanBeReturnedIfOpened", SqlDbType.Bit).Value = entity.CanBeReturnedIfOpened;
            command.Parameters.Add("@NearExpiryThresholdInDays", SqlDbType.Int).Value = entity.NearExpiryThresholdInDays;
            command.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)entity.Status;
            command.Parameters.Add("@ImageStorageKey", SqlDbType.NVarChar, 500).Value = entity.Image.StorageKey;
            command.Parameters.Add("@ImageFileName", SqlDbType.NVarChar, 100).Value = entity.Image.FileName;
            command.Parameters.Add("@ImageMimeType", SqlDbType.NVarChar, 50).Value = entity.Image.MimeType;

            try
            {
                return await command.ExecuteScalarAsync(cancellationToken) as byte[]
                    ?? throw new InvalidOperationException("Failed to retrieve RowVersion after inserting the Product. Check the SQL OUTPUT clause.");
            }
            catch (SqlException ex) when (SqlErrors.IsUniqueViolation(ex, "UX_Products_Sku"))
            {
                throw new DuplicateSkuException(entity.SKU.Value);
            }
        }

        private static async Task InsertPackagingVersionGraphAsync(Guid productId, PackagingVersion packagingVersion, SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken)
        {
            await using (SqlCommand versionCommand = new(insertPackagingVersionSql, connection, transaction))
            {
                versionCommand.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = packagingVersion.PackagingVersionId.Value;
                versionCommand.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = productId;
                versionCommand.Parameters.Add("@Version", SqlDbType.Int).Value = packagingVersion.Version;
                versionCommand.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)packagingVersion.Status;
                await versionCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            foreach (ProductUnitDetails unit in packagingVersion.Units)
            {
                int productUnitId;
                await using (SqlCommand unitCommand = new(insertProductUnitSql, connection, transaction))
                {
                    unitCommand.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = packagingVersion.PackagingVersionId.Value;
                    unitCommand.Parameters.Add("@Unit", SqlDbType.TinyInt).Value = (byte)unit.Unit;
                    unitCommand.Parameters.Add("@ConversionRate", SqlDbType.Int).Value = unit.ConversionRate;
                    unitCommand.Parameters.Add("@Rank", SqlDbType.TinyInt).Value = (byte)unit.Rank;
                    productUnitId = await unitCommand.ExecuteScalarAsync(cancellationToken) is int id
                        ? id
                        : throw new InvalidOperationException("Failed to retrieve ProductUnitId after inserting the ProductUnit. Check the SQL OUTPUT clause.");
                }

                foreach (Barcode barcode in unit.Barcodes)
                    await InsertBarcodeRowAsync(productUnitId, barcode, connection, transaction, cancellationToken);
            }
        }

        private static async Task InsertBarcodeRowAsync(int productUnitId, Barcode barcode, SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken)
        {
            await using SqlCommand command = new(insertBarcodeSql, connection, transaction);
            command.Parameters.Add("@ProductUnitId", SqlDbType.Int).Value = productUnitId;
            command.Parameters.Add("@Value", SqlDbType.NVarChar, 50).Value = barcode.Value;
            command.Parameters.Add("@Type", SqlDbType.TinyInt).Value = (byte)barcode.Type;

            try
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (SqlException ex) when (SqlErrors.IsUniqueViolation(ex, "UX_Barcodes_Value"))
            {
                throw new DuplicateBarcodeException(barcode.Value);
            }
        }

        public async Task<bool> ExistsByBarcodeAsync(Barcode barcode, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(barcode, nameof(barcode));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(ExistsByBarcodeSql, connection);
            command.Parameters.Add("@Value", SqlDbType.NVarChar, 50).Value = barcode.Value;

            object? result = await command.ExecuteScalarAsync(cancellationToken);

            return result is not null;
        }

        public async Task<bool> ExistsBySkuAsync(SKU sku, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(sku, nameof(sku));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(ExistsBySkuSql, connection);
            command.Parameters.Add("@Sku", SqlDbType.NVarChar, 50).Value = sku.Value;

            object? result = await command.ExecuteScalarAsync(cancellationToken);

            return result is not null;
        }

        public async Task<(Product? Entity, byte[]? RowVersion)> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(GetByIdSql, connection);
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

            newRowVersion = productFieldsChanged
                ? await UpdateProductFieldsAsync(entity, expectedRowVersion, connection, transaction, cancellationToken)
                : await TouchProductRowVersionAsync(entity.ProductId, expectedRowVersion, connection, transaction, cancellationToken);

            foreach (var domainEvent in entity.DomainEvents)
            {
                switch (domainEvent)
                {
                    case PackagingVersionStatusEvent e:
                        {
                            await UpdatePackagingVersionStatusAsync(e, connection, transaction, cancellationToken);
                            break;
                        }

                    case PackagingVersionAddedEvent e:
                        {
                            PackagingVersion version = entity.PackagingVersions.Single(v => v.PackagingVersionId == e.PackagingVersionId);
                            await InsertPackagingVersionGraphAsync(e.ProductId.Value, version, connection, transaction, cancellationToken);
                            break;
                        }

                    case CompanyBarcodeAddedEvent e:
                        {
                            var productUnitId = await FindProductUnitIdAsync(e.PackagingVersionId, e.unit, connection, transaction, cancellationToken);
                            await InsertBarcodeRowAsync(productUnitId, e.barcode, connection, transaction, cancellationToken);
                            break;
                        }

                    case CompanyBarcodeRemovedEvent e:
                        {
                            await DeleteBarcodeAsync(e, connection, transaction, cancellationToken);
                            break;
                        }
                }
            }

            await transaction.CommitAsync(cancellationToken);
            entity.ClearDomainEvents();

            return newRowVersion;
        }

        private static async Task<int> FindProductUnitIdAsync(PackagingVersionId packagingVersionId, UnitOfMeasure unit, SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken)
        {
            await using SqlCommand command = new(FindProductUnitIdSql, connection, transaction);
            command.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = packagingVersionId.Value;
            command.Parameters.Add("@Unit", SqlDbType.TinyInt).Value = (byte)unit;

            return await command.ExecuteScalarAsync(cancellationToken) is int id
                ? id
                : throw new InvalidOperationException($"ProductUnit not found for PackagingVersionId {packagingVersionId.Value} and Unit {unit}.");
        }

        private static async Task<byte[]> TouchProductRowVersionAsync(
            ProductId productId,
            byte[] expectedRowVersion,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            await using SqlCommand command = new(TouchProductRowVersionSql, connection, transaction);

            command.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value = productId.Value;
            command.Parameters.Add("@ExpectedRowVersion", SqlDbType.Timestamp).Value = expectedRowVersion;

            object? result = await command.ExecuteScalarAsync(cancellationToken);

            if (result is not byte[] newRowVersion)
                throw new ConcurrencyConflictException(productId.Value);

            return newRowVersion;
        }

        private static async Task<byte[]> UpdateProductFieldsAsync(
            Product entity,
            byte[] expectedRowVersion,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            await using SqlCommand command = new(UpdateProductFieldsSql, connection, transaction);

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
                throw new ConcurrencyConflictException(entity.ProductId.Value);

            return newRowVersion;
        }

        private static async Task UpdatePackagingVersionStatusAsync(
            PackagingVersionStatusEvent domainEvent,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            await using SqlCommand command = new(UpdatePackagingVersionStatusSql, connection, transaction);
            command.Parameters.Add("@Status", SqlDbType.TinyInt).Value = (byte)domainEvent.newStatus;
            command.Parameters.Add("@PackagingVersionId", SqlDbType.UniqueIdentifier).Value = domainEvent.PackagingVersionId.Value;

            int affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);

            if (affectedRows != 1)
                throw new InvalidOperationException(
                    $"PackagingVersion {domainEvent.PackagingVersionId.Value} was not found during status update.");
        }

        private static async Task DeleteBarcodeAsync(
            CompanyBarcodeRemovedEvent domainEvent,
            SqlConnection connection,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            await using SqlCommand command = new(DeleteBarcodeSql, connection, transaction);
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
