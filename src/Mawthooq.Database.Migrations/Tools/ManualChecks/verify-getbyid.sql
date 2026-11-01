BEGIN TRANSACTION;

DECLARE @c  UNIQUEIDENTIFIER = NEWID(), @m UNIQUEIDENTIFIER = NEWID(), @a UNIQUEIDENTIFIER = NEWID();
DECLARE @p  UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @v1 UNIQUEIDENTIFIER = NEWID(), @v2 UNIQUEIDENTIFIER = NEWID();
DECLARE @u  INT;

INSERT dbo.Categories (CategoryId, Name, Description, Status) VALUES (@c, N'VerifyCat', N'Verify', 1);
INSERT dbo.Manufacturers (ManufacturerId, Name, Status) VALUES (@m, N'VerifyMaker', 1);
INSERT dbo.LocalAgents (LocalAgentId, Name, Status) VALUES (@a, N'VerifyAgent', 1);

INSERT dbo.Products (ProductId, TradeName, Sku, CategoryId, ManufacturerId, LocalAgentId, RequiresExpiryTracking,
    CanBeReturnedIfOpened, NearExpiryThresholdInDays, Status, ImageStorageKey, ImageFileName, ImageMimeType)
VALUES (@p, N'Panadol', N'SKU-VERIFY1', @c, @m, @a, 1, 1, 90, 1, N'k', N'f.png', N'image/png');

-- version 2 الأول (ترتيب مقلوب)
INSERT dbo.PackagingVersions (PackagingVersionId, ProductId, Version, Status) VALUES (@v2, @p, 2, 1);
INSERT dbo.ProductUnits (PackagingVersionId, Unit, ConversionRate, Rank) VALUES (@v2, 4, 1, 1);          -- Box
SET @u = SCOPE_IDENTITY();
INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, N'1003', 6);

-- version 1: الـ Disc (rank 3) الأول، وبعدين Box (rank 1)، وبعدين Strip (rank 2)
INSERT dbo.PackagingVersions (PackagingVersionId, ProductId, Version, Status) VALUES (@v1, @p, 1, 1);

INSERT dbo.ProductUnits (PackagingVersionId, Unit, ConversionRate, Rank) VALUES (@v1, 1, 1, 3);         -- Disc
SET @u = SCOPE_IDENTITY();
INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, N'1002', 6);

INSERT dbo.ProductUnits (PackagingVersionId, Unit, ConversionRate, Rank) VALUES (@v1, 4, 120, 1);       -- Box
SET @u = SCOPE_IDENTITY();
INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, N'1000', 6);                               -- internal الأول
INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, N'5000000000017', 1);                      -- EAN13 بعده

INSERT dbo.ProductUnits (PackagingVersionId, Unit, ConversionRate, Rank) VALUES (@v1, 3, 12, 2);        -- Strip
SET @u = SCOPE_IDENTITY();
INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, N'1001', 6);

-- ==================================
DECLARE @ProductId UNIQUEIDENTIFIER = @p;

/**p.ProductId, p.TradeName, p.Sku, p.CategoryId, p.ManufacturerId, p.LocalAgentId,
    p.RequiresExpiryTracking, p.CanBeReturnedIfOpened, p.NearExpiryThresholdInDays,
    p.Status AS ProductStatus, p.ImageStorageKey, p.ImageFileName, p.ImageMimeType, p.RowVersion,
    pv.PackagingVersionId, pv.Version, pv.Status AS PackagingVersionStatus,
    pu.ProductUnitId, pu.Unit, pu.ConversionRate, pu.Rank,
    b.Value AS BarcodeValue, b.Type AS BarcodeType
    **/

SELECT
    pv.Version,
    pu.Rank, pu.Unit, pu.ConversionRate,
    b.Value AS BarcodeValue, b.Type AS BarcodeType

FROM
    dbo.Products p

INNER JOIN
    dbo.PackagingVersions pv
ON pv.ProductId = p.ProductId

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

SELECT
    COUNT(*) AS RowsForUnknownProduct

FROM
    dbo.Products p
INNER JOIN
    dbo.PackagingVersions pv
ON
    pv.ProductId = p.ProductId

WHERE
    p.ProductId = NEWID();

ROLLBACK TRANSACTION;

-- Result

/**
Version     Rank Unit ConversionRate BarcodeValue                                       BarcodeType
----------- ---- ---- -------------- -------------------------------------------------- -----------
          1    1    4            120 1000                                                         6
          1    1    4            120 5000000000017                                                1
          1    2    3             12 1001                                                         6
          1    3    1              1 1002                                                         6
          2    1    4              1 1003                                                         6

(5 rows affected)
RowsForUnknownProduct
---------------------
                    0
**/
