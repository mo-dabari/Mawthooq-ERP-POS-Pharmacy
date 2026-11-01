CREATE TABLE
    dbo.Products (
        ProductId UNIQUEIDENTIFIER NOT NULL,
        TradeName NVARCHAR (100) NOT NULL,
        Sku NVARCHAR (50) NOT NULL,
        CategoryId UNIQUEIDENTIFIER NOT NULL,
        ManufacturerId UNIQUEIDENTIFIER NOT NULL,
        LocalAgentId UNIQUEIDENTIFIER NOT NULL,
        RequiresExpiryTracking BIT NOT NULL,
        CanBeReturnedIfOpened BIT NOT NULL,
        NearExpiryThresholdInDays INT NOT NULL,
        Status TINYINT NOT NULL,
        ImageStorageKey NVARCHAR (500) NOT NULL,
        ImageFileName NVARCHAR (100) NOT NULL,
        ImageMimeType NVARCHAR (50) NOT NULL,
        RowVersion ROWVERSION NOT NULL,

        CONSTRAINT PK_Products PRIMARY KEY CLUSTERED (ProductId),
        CONSTRAINT CK_Products_Status CHECK (Status IN (1, 2)),
        CONSTRAINT CK_Products_NearExpiryThreshold CHECK (
            (
                RequiresExpiryTracking = 0
                AND NearExpiryThresholdInDays = 0
            )
            OR (
                RequiresExpiryTracking = 1
                AND NearExpiryThresholdInDays >= 90
            )
        ),
        CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories (CategoryId),
        CONSTRAINT FK_Products_Manufacturers FOREIGN KEY (ManufacturerId) REFERENCES dbo.Manufacturers (ManufacturerId),
        CONSTRAINT FK_Products_LocalAgents FOREIGN KEY (LocalAgentId) REFERENCES dbo.LocalAgents (LocalAgentId)
    );

GO

CREATE UNIQUE INDEX UX_Products_Sku ON dbo.Products (Sku);

CREATE INDEX IX_Products_CategoryId ON dbo.Products (CategoryId);

CREATE INDEX IX_Products_ManufacturerId ON dbo.Products (ManufacturerId);

CREATE INDEX IX_Products_Status ON dbo.Products (Status);

CREATE INDEX IX_Products_TradeName ON dbo.Products (TradeName);

GO
