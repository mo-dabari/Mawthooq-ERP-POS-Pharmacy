-- =============================================
-- Barcodes (Value Object collection, child of ProductUnit)
-- Corresponds to Domain's Barcode hierarchy (InternalLinearBarcode,
-- BarcodeEAN13Linear, BarcodeQR). Barcode values are globally unique
-- across the entire catalog — this mirrors IProductRepository
-- .ExistsByBarcodeAsync, which checks uniqueness without scoping to
-- a single product.
-- =============================================

CREATE TABLE dbo.Barcodes
(
    BarcodeId        INT IDENTITY(1,1)    NOT NULL,
    ProductUnitId      INT                   NOT NULL,
    Value                NVARCHAR(50)          NOT NULL,
    Type                  TINYINT               NOT NULL,

    CONSTRAINT PK_Barcodes PRIMARY KEY CLUSTERED (BarcodeId),

    CONSTRAINT FK_Barcodes_ProductUnits
        FOREIGN KEY (ProductUnitId) REFERENCES dbo.ProductUnits (ProductUnitId)
        ON DELETE CASCADE,

    -- BarcodeType enum: 1=EAN13, 2=UPC, 3=Code128, 4=QRCode, 5=DataMatrix, 6=Internal
    CONSTRAINT CK_Barcodes_Type CHECK (Type IN (1, 2, 3, 4, 5, 6))
);
GO

-- Global uniqueness — supports both the Domain invariant and
-- ScanProductBarcode / AddingNewBarcodeToUnit duplicate checks.
CREATE UNIQUE INDEX UX_Barcodes_Value ON dbo.Barcodes (Value);
GO

CREATE INDEX IX_Barcodes_ProductUnitId ON dbo.Barcodes (ProductUnitId);
GO
