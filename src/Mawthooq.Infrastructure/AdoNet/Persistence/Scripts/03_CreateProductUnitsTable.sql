-- =============================================
-- ProductUnits (Value Object collection, child of PackagingVersion)
-- Corresponds to Domain's ProductUnitDetails. Uses a surrogate INT
-- IDENTITY key since the Domain itself has no identity for this
-- object — it's a Value Object, not an Entity.
-- =============================================

CREATE TABLE dbo.ProductUnits
(
    ProductUnitId           INT IDENTITY(1,1)    NOT NULL,
    PackagingVersionId       UNIQUEIDENTIFIER      NOT NULL,
    Unit                       TINYINT               NOT NULL,
    ConversionRate              INT                   NOT NULL,
    Rank                         TINYINT               NOT NULL,

    CONSTRAINT PK_ProductUnits PRIMARY KEY CLUSTERED (ProductUnitId),

    CONSTRAINT FK_ProductUnits_PackagingVersions
        FOREIGN KEY (PackagingVersionId) REFERENCES dbo.PackagingVersions (PackagingVersionId)
        ON DELETE CASCADE,

    CONSTRAINT CK_ProductUnits_ConversionRate CHECK (ConversionRate > 0),

    -- UnitRank enum: 1 = Primary, 2 = Secondary, 3 = Tertiary
    CONSTRAINT CK_ProductUnits_Rank CHECK (Rank IN (1, 2, 3)),

    -- Mirrors PackagingVersion.CreateUnits domain invariants:
    -- no duplicate Unit, no duplicate Rank within the same version.
    CONSTRAINT UX_ProductUnits_Version_Unit UNIQUE (PackagingVersionId, Unit),
    CONSTRAINT UX_ProductUnits_Version_Rank UNIQUE (PackagingVersionId, Rank)
);
GO

CREATE INDEX IX_ProductUnits_PackagingVersionId ON dbo.ProductUnits (PackagingVersionId);
GO
