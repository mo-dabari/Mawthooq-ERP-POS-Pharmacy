-- =============================================
-- Products (Aggregate Root)
-- Concurrency: RowVersion column, checked on every UPDATE.
-- Note: CategoryId, ManufacturerId, LocalAgentId are stored as plain
-- UNIQUEIDENTIFIER (no FK constraint yet) because the Category,
-- Manufacturer, and LocalAgent tables are outside the Product feature
-- scope and haven't been reviewed/created yet. FK constraints should
-- be added once those tables exist.
-- =============================================

CREATE TABLE dbo.Products
(
    ProductId                  UNIQUEIDENTIFIER    NOT NULL,
    TradeName                  NVARCHAR(100)       NOT NULL,
    Sku                        NVARCHAR(50)        NOT NULL,
    CategoryId                 UNIQUEIDENTIFIER    NOT NULL,
    ManufacturerId              UNIQUEIDENTIFIER    NOT NULL,
    LocalAgentId                 UNIQUEIDENTIFIER    NOT NULL,
    RequiresExpiryTracking       BIT                 NOT NULL,
    CanBeReturnedIfOpened         BIT                 NOT NULL,
    NearExpiryThresholdInDays      INT                 NOT NULL,
    Status                          TINYINT             NOT NULL,
    ImageStorageKey                  NVARCHAR(500)       NOT NULL,
    ImageFileName                     NVARCHAR(100)       NOT NULL,
    ImageMimeType                      NVARCHAR(50)       NOT NULL,
    RowVersion                          ROWVERSION,

    CONSTRAINT PK_Products PRIMARY KEY CLUSTERED (ProductId),

    -- Status enum: 1 = Active, 2 = Inactive (Mawthooq.Domain.SharedKernel.Enums.Status)
    CONSTRAINT CK_Products_Status CHECK (Status IN (1, 2)),

    CONSTRAINT CK_Products_NearExpiryThreshold CHECK (NearExpiryThresholdInDays >= 0)
);
GO

-- SKU must be unique (enforced in Domain via IProductRepository.ExistsBySkuAsync,
-- enforced here as the source of truth against race conditions).
CREATE UNIQUE INDEX UX_Products_Sku ON dbo.Products (Sku);
GO

-- Supports GetProducts filtering (nقطة 18 من الـ Handoff Report).
CREATE INDEX IX_Products_CategoryId ON dbo.Products (CategoryId);
CREATE INDEX IX_Products_ManufacturerId ON dbo.Products (ManufacturerId);
CREATE INDEX IX_Products_Status ON dbo.Products (Status);

-- Supports SearchProducts / GetProductLookup typeahead (LIKE 'text%').
CREATE INDEX IX_Products_TradeName ON dbo.Products (TradeName);
GO
