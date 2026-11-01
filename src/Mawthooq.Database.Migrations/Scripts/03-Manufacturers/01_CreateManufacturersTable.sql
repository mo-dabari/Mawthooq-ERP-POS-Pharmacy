CREATE TABLE
    dbo.Manufacturers (
        ManufacturerId UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR (50) NOT NULL,
        Status TINYINT NOT NULL CONSTRAINT CK_Manufacturers_Status CHECK (Status IN (1, 2)),
        RowVersion ROWVERSION NOT NULL,

        CONSTRAINT PK_Manufacturers PRIMARY KEY CLUSTERED (ManufacturerId),
        
        CONSTRAINT UX_Manufacturers_Name UNIQUE (Name)
    );

Go
