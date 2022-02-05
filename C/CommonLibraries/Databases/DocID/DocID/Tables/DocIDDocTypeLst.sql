CREATE TABLE [dbo].[DocIDDocTypeLst] (
    [DocID]        VARCHAR (50)  NOT NULL,
    [DocumentType] VARCHAR (500) NOT NULL,
    [CompassOnlyArc] VARCHAR(5) NULL, 
    CONSTRAINT [PK_DocIDDocTypeLst] PRIMARY KEY CLUSTERED ([DocID] ASC, [DocumentType] ASC)
);

