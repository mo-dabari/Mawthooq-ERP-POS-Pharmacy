CREATE TABLE
    dbo.LocalAgents (
        LocalAgentId UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR (50) NOT NULL,
        Status TINYINT NOT NULL CONSTRAINT CK_LocalAgents_Status CHECK (Status IN (1, 2)),
        RowVersion ROWVERSION NOT NULL,

        CONSTRAINT PK_LocalAgents PRIMARY KEY CLUSTERED (LocalAgentId),

        CONSTRAINT UX_LocalAgents_Name UNIQUE (Name)
    );

GO
