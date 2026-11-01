SET XACT_ABORT OFF;

DECLARE @c UNIQUEIDENTIFIER = NEWID(), @m UNIQUEIDENTIFIER = NEWID(), @a UNIQUEIDENTIFIER = NEWID(), @p UNIQUEIDENTIFIER = NEWID();

INSERT dbo.Categories (CategoryId, Name, Description, Status) VALUES (@c, N'Pain', N'Pain relief', 1);
INSERT dbo.Manufacturers (ManufacturerId, Name, Status) VALUES (@m, N'MakerA', 1);
INSERT dbo.LocalAgents (LocalAgentId, Name, Status) VALUES (@a, N'AgentA', 1);

-- 1) متوقع: ينجح
INSERT dbo.Products (ProductId, TradeName, Sku, CategoryId, ManufacturerId, LocalAgentId, RequiresExpiryTracking,
    CanBeReturnedIfOpened, NearExpiryThresholdInDays, Status, ImageStorageKey, ImageFileName, ImageMimeType)
VALUES (@p, N'Panadol', N'SKU-0001', @c, @m, @a, 1, 0, 90, 1, N'k', N'f.png', N'image/png');

-- 2) متوقع: يفشل CK_Products_ExpiryPolicy (من غير tracking وعنده 45)
INSERT dbo.Products (ProductId, TradeName, Sku, CategoryId, ManufacturerId, LocalAgentId, RequiresExpiryTracking,
    CanBeReturnedIfOpened, NearExpiryThresholdInDays, Status, ImageStorageKey, ImageFileName, ImageMimeType)
VALUES (NEWID(), N'Device', N'SKU-0002', @c, @m, @a, 0, 0, 45, 1, N'k', N'f.png', N'image/png');

-- 3) متوقع: يفشل FK_Products_Categories (category مش موجودة)
INSERT dbo.Products (ProductId, TradeName, Sku, CategoryId, ManufacturerId, LocalAgentId, RequiresExpiryTracking,
    CanBeReturnedIfOpened, NearExpiryThresholdInDays, Status, ImageStorageKey, ImageFileName, ImageMimeType)
VALUES (NEWID(), N'Ghost', N'SKU-0003', NEWID(), @m, @a, 1, 0, 90, 1, N'k', N'f.png', N'image/png');

-- 4) متوقع: يفشل UX_Products_Sku (نفس الـ SKU)
INSERT dbo.Products (ProductId, TradeName, Sku, CategoryId, ManufacturerId, LocalAgentId, RequiresExpiryTracking,
    CanBeReturnedIfOpened, NearExpiryThresholdInDays, Status, ImageStorageKey, ImageFileName, ImageMimeType)
VALUES (NEWID(), N'Dup', N'SKU-0001', @c, @m, @a, 1, 0, 90, 1, N'k', N'f.png', N'image/png');

-- 5) باركودات: Ab12 وـ aB12 المتوقع إنهم الاتنين ينجحوا (BIN2)، وـ Ab12 تاني يفشل UX_Barcodes_Value
DECLARE @v UNIQUEIDENTIFIER = NEWID();
INSERT dbo.PackagingVersions (PackagingVersionId, ProductId, Version, Status) VALUES (@v, @p, 1, 1);
INSERT dbo.ProductUnits (PackagingVersionId, Unit, ConversionRate, Rank) VALUES (@v, 1, 1, 1);
DECLARE @u INT = SCOPE_IDENTITY();
INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, N'Ab12', 4);
INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, N'aB12', 4);
INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, N'Ab12', 4);
GO

-- 6) الـ sequence: متوقع 1000، وبعدين الـ range اللي بتستخدمه الـ generator
SELECT NEXT VALUE FOR dbo.ProductBarcodeSequence AS FirstValue;          -- 1000

DECLARE @first SQL_VARIANT;

EXEC sp_sequence_get_range @sequence_name = N'dbo.ProductBarcodeSequence', @range_size = 3, @range_first_value = @first OUTPUT;
SELECT CAST(@first AS BIGINT) AS RangeFirst;                              -- 1001



