BEGIN TRANSACTION;

DECLARE @c UNIQUEIDENTIFIER = NEWID(), @m UNIQUEIDENTIFIER = NEWID(), @a UNIQUEIDENTIFIER = NEWID();
INSERT dbo.Categories (CategoryId, Name, Description, Status) VALUES (@c, N'C', N'C', 1);
INSERT dbo.Manufacturers (ManufacturerId, Name, Status) VALUES (@m, N'M', 1);
INSERT dbo.LocalAgents (LocalAgentId, Name, Status) VALUES (@a, N'A', 1);

DECLARE @p1 UNIQUEIDENTIFIER = NEWID(), @p2 UNIQUEIDENTIFIER = NEWID(), @p3 UNIQUEIDENTIFIER = NEWID(), @p4 UNIQUEIDENTIFIER = NEWID();
INSERT dbo.Products (ProductId, TradeName, Sku, CategoryId, ManufacturerId, LocalAgentId, RequiresExpiryTracking, CanBeReturnedIfOpened, NearExpiryThresholdInDays, Status, ImageStorageKey, ImageFileName, ImageMimeType) VALUES
 (@p1, N'P1 active',          N'VER-0001', @c, @m, @a, 1, 1, 90, 1, N'k', N'f', N'image/png'),
 (@p2, N'P2 two versions',    N'VER-0002', @c, @m, @a, 1, 1, 90, 1, N'k', N'f', N'image/png'),
 (@p3, N'P3 all inactive',    N'VER-0003', @c, @m, @a, 1, 1, 90, 1, N'k', N'f', N'image/png'),
 (@p4, N'Betadine 10%',       N'VER-0004', @c, @m, @a, 1, 1, 90, 1, N'k', N'f', N'image/png');

-- (productId, version, status, internal barcode): كل version فيها unit واحدة rank 1
DECLARE @t TABLE (ProductId UNIQUEIDENTIFIER, Ver INT, St TINYINT, Code NVARCHAR(50));
INSERT @t VALUES (@p1,1,1,N'1000'), (@p2,1,2,N'2000'), (@p2,2,1,N'2001'), (@p3,1,2,N'3000'), (@p4,1,1,N'4000');

DECLARE @pid UNIQUEIDENTIFIER, @ver INT, @st TINYINT, @code NVARCHAR(50), @v UNIQUEIDENTIFIER, @u INT;
DECLARE cur CURSOR FOR SELECT ProductId, Ver, St, Code FROM @t;
OPEN cur; FETCH NEXT FROM cur INTO @pid, @ver, @st, @code;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @v = NEWID();
    INSERT dbo.PackagingVersions (PackagingVersionId, ProductId, Version, Status) VALUES (@v, @pid, @ver, @st);
    INSERT dbo.ProductUnits (PackagingVersionId, Unit, ConversionRate, Rank) VALUES (@v, 42, 1, 1);   -- Piece
    SET @u = SCOPE_IDENTITY();
    INSERT dbo.Barcodes (ProductUnitId, Value, Type) VALUES (@u, @code, 6);
    FETCH NEXT FROM cur INTO @pid, @ver, @st, @code;
END
CLOSE cur; DEALLOCATE cur;

-- (1) الـ CROSS APPLY الجديد: لازم 4 صفوف. P2 → 2001 (الـ active)، P3 → 3000 (الاحتياطي)
SELECT p.TradeName, ib.Value AS InternalBarcode
FROM dbo.Products p
CROSS APPLY (
    SELECT TOP (1) b.Value
    FROM dbo.PackagingVersions pv
    INNER JOIN dbo.ProductUnits pu ON pu.PackagingVersionId = pv.PackagingVersionId AND pu.Rank = 1
    INNER JOIN dbo.Barcodes b ON b.ProductUnitId = pu.ProductUnitId AND b.Type = 6
    WHERE pv.ProductId = p.ProductId
    ORDER BY CASE WHEN pv.Status = 1 THEN 0 ELSE 1 END, pv.Version DESC
) ib
WHERE p.Sku LIKE N'VER-%'
ORDER BY p.TradeName, p.ProductId;

-- (2) بحث "يحتوي" بـ 10% مع ESCAPE: لازم صف واحد (Betadine 10%)
SELECT TradeName FROM dbo.Products WHERE TradeName LIKE N'%10\%%' ESCAPE '\' AND Sku LIKE N'VER-%';

-- (3) نفس البحث من غير ESCAPE: الـ % wildcard (لازم يطلع Betadine 10% بس، لأن مفيش منتج تاني فيه 10)
SELECT TradeName FROM dbo.Products WHERE TradeName LIKE N'%10%%' AND Sku LIKE N'VER-%';

ROLLBACK TRANSACTION;


BEGIN TRANSACTION;
INSERT dbo.Categories (CategoryId, Name, Description, Status) VALUES (NEWID(), N'Pain', N'x', 1);
INSERT dbo.Categories (CategoryId, Name, Description, Status) VALUES (NEWID(), N'pain', N'y', 1);   
ROLLBACK TRANSACTION;
