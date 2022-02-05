CREATE TABLE [dbo].[DocTypeFunctionalityChecks] (
    [DocType]           VARCHAR (500) NOT NULL,
    [DoLSandSChecks]    VARCHAR (50)  NOT NULL,
    [DoBankruptcyCheck] VARCHAR (50)  NOT NULL,
    [DoGeneralCorr]     VARCHAR (50)  NOT NULL,
    [DoLVC]             VARCHAR (50)  NOT NULL,
    [DocIDTranslation]  VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_DocTypeFunctionalityChecks] PRIMARY KEY CLUSTERED ([DocType] ASC)
);

