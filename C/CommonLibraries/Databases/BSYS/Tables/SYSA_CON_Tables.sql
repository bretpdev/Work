CREATE TABLE [dbo].[SYSA_CON_Tables] (
    [TableName]       VARCHAR (100) NOT NULL,
    [TableType]       VARCHAR (50)  CONSTRAINT [DF_SYSA_CON_Tables_IsSystemAccess] DEFAULT ((0)) NOT NULL,
    [DataDescription] VARCHAR (MAX) NULL,
    [NewDatabase]     VARCHAR (MAX) NULL,
    [NewTableName]    VARCHAR (100) NULL,
    [Notes]           VARCHAR (MAX) NULL,
    CONSTRAINT [PK_SYSA_CON_Tables] PRIMARY KEY CLUSTERED ([TableName] ASC)
);

