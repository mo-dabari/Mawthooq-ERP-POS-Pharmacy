CREATE TABLE
    dbo.Categories (
        CategoryId UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR (100) NOT NULL,
        Description NVARCHAR (500) NOT NULL,
        Status TINYINT NOT NULL CONSTRAINT CK_Categories_Status CHECK (Status IN (1, 2)),
        RowVersion ROWVERSION NOT NULL,

        CONSTRAINT PK_Categories PRIMARY KEY CLUSTERED (CategoryId),
        
        CONSTRAINT UX_Categories_Name UNIQUE (Name)
    );

Go
