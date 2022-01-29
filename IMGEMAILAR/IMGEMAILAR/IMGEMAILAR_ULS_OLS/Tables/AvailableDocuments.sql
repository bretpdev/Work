CREATE TABLE [imgemailar].[AvailableDocuments]
(
	[AvailableDocumentId] INT NOT NULL PRIMARY KEY IDENTITY,
	[LetterId] VARCHAR(10), 
	[OverrideDescription] VARCHAR(50)
)
