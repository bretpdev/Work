CREATE TABLE [dbo].[SYSA_LST_Systems] (
    [System]             NVARCHAR (30) NOT NULL,
    [System Description] NTEXT         NULL,
    [UsesUTID]           CHAR (1)      NOT NULL,
    [UsesGroupAccess]    CHAR (1)      NOT NULL,
    CONSTRAINT [PK_SYSA_LST_Systems] PRIMARY KEY CLUSTERED ([System] ASC)
);

