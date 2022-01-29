CREATE TABLE [dbo].[EntityTypeAttributeAllowedValuesHistory]
(
	[EntityTypeAttributeAllowedValuesHistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EntityTypeAttributeId] INT NOT NULL, 
    [ValueId] INT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [HistoryStatusTypeId] INT NOT NULL, 
    [HistoryStatusDate] DATETIME NOT NULL DEFAULT (getdate()), 
    [HistoryStatusCreatedBy] INT NOT NULL
)
