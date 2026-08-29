-- =============================================
-- PackagingVersions (Entity, child of Product)
-- One Product can have multiple versions over time (historical
-- correctness — see Handoff Report section 14). Only ever inserted
-- or status-updated, never mutated in place for Units/ConversionRate.
-- =============================================

CREATE TABLE dbo.PackagingVersions
(
    PackagingVersionId     UNIQUEIDENTIFIER    NOT NULL,
    ProductId               UNIQUEIDENTIFIER    NOT NULL,
    Version                 INT                 NOT NULL,
    Status                   TINYINT             NOT NULL,

    CONSTRAINT PK_PackagingVersions PRIMARY KEY CLUSTERED (PackagingVersionId),

    CONSTRAINT FK_PackagingVersions_Products
        FOREIGN KEY (ProductId) REFERENCES dbo.Products (ProductId)
        ON DELETE CASCADE,

    CONSTRAINT CK_PackagingVersions_Status CHECK (Status IN (1, 2)),

    CONSTRAINT CK_PackagingVersions_Version CHECK (Version > 0),

    -- Mirrors the Domain rule: version numbers are sequential per product.
    CONSTRAINT UX_PackagingVersions_Product_Version UNIQUE (ProductId, Version)
);
GO

CREATE INDEX IX_PackagingVersions_ProductId ON dbo.PackagingVersions (ProductId);
GO
