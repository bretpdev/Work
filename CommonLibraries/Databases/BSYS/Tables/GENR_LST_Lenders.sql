CREATE TABLE [dbo].[GENR_LST_Lenders] (
    [Name]     VARCHAR (50)  NOT NULL,
    [Address1] VARCHAR (100) NOT NULL,
    [Address2] VARCHAR (100) NULL,
    [City]     VARCHAR (50)  NOT NULL,
    [State]    VARCHAR (50)  NULL,
    [Zip]      VARCHAR (20)  NOT NULL,
    [Country]  VARCHAR (50)  NULL,
    CONSTRAINT [PK_GENR_LST_Lenders] PRIMARY KEY CLUSTERED ([Name] ASC)
);

