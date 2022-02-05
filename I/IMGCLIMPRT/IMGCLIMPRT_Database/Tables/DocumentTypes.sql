CREATE TABLE [imgclimprt].[DocumentTypes]
(
	[DocumentTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DocumentTypeValue] VARCHAR(50) NOT NULL, 
    [DocIdId] INT NOT NULL, 
    CONSTRAINT [FK_DocumentTypes_DocIds] FOREIGN KEY ([DocIdId]) REFERENCES [imgclimprt].[DocIds]([DocIdId])
)
