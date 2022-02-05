CREATE TABLE [dbo].[SYSA_CON_ApplicationTables] (
    [Application] VARCHAR (100) NOT NULL,
    [TableName]   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_SYSA_CON_ApplicationTables] PRIMARY KEY CLUSTERED ([Application] ASC, [TableName] ASC)
);

