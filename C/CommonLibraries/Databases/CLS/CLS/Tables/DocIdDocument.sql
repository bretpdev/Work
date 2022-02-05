CREATE TABLE [dbo].[DocIdDocument] (
    [DocId]       VARCHAR (5)   NOT NULL,
    [Description] VARCHAR (500) NOT NULL,
    [Arc]         VARCHAR (5)   NOT NULL,
    CONSTRAINT [PK_DocIdDocument] PRIMARY KEY CLUSTERED ([DocId] ASC, [Description] ASC)
);

