CREATE TABLE [dbo].[BatchPasswordBase]
(
	[BatchPasswordBaseId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BaseWord] VARBINARY(128) NOT NULL, 
    [Format] VARBINARY(128) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [InactivatedAt] DATETIME NULL
)
