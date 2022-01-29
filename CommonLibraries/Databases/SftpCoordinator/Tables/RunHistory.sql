CREATE TABLE [dbo].[RunHistory]
(
	[RunHistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StartedOn] DATETIME NOT NULL DEFAULT getdate(), 
	[EndedOn] DATETIME NULL,
    [RunBy] NVARCHAR(50) NOT NULL
)
