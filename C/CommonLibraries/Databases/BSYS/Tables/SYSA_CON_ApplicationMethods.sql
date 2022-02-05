CREATE TABLE [dbo].[SYSA_CON_ApplicationMethods] (
    [Application] VARCHAR (100) NOT NULL,
    [Library]     VARCHAR (50)  NOT NULL,
    [Method]      VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_SystemAccessConversion_ApplicationMethods] PRIMARY KEY CLUSTERED ([Application] ASC, [Method] ASC, [Library] ASC)
);

