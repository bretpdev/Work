CREATE TABLE [dbo].[SYSA_CON_MethodTables] (
    [Library]   VARCHAR (50)  NOT NULL,
    [Method]    VARCHAR (50)  NOT NULL,
    [TableName] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_SystemAccessConversion_Methods] PRIMARY KEY CLUSTERED ([Library] ASC, [Method] ASC, [TableName] ASC)
);

