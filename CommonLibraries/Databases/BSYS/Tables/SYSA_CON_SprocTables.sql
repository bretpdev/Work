CREATE TABLE [dbo].[SYSA_CON_SprocTables] (
    [Sproc]     VARCHAR (100) NOT NULL,
    [TableName] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_SYSA_CON_SprocTables] PRIMARY KEY CLUSTERED ([Sproc] ASC, [TableName] ASC)
);

