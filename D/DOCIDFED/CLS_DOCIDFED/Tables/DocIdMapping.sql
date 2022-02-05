CREATE TABLE [docid].[DocIdMapping]
(
	[DocIdMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DocumentId] INT NOT NULL, 
    [DocumentTypeId] INT NOT NULL, 
    [ArcId] INT NOT NULL, 
    CONSTRAINT [FK_DocIdMapping_Documents] FOREIGN KEY ([DocumentId]) REFERENCES [docid].[Documents]([DocumentsId]), 
    CONSTRAINT [FK_DocIdMapping_DocumentTypes] FOREIGN KEY ([DocumentTypeId]) REFERENCES [docid].[DocumentTypes]([DocumentTypesId]), 
    CONSTRAINT [FK_DocIdMapping_Arcs] FOREIGN KEY ([ArcId]) REFERENCES [docid].[Arcs]([ArcId])
)
