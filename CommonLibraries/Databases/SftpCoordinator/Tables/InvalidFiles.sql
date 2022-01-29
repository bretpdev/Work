CREATE TABLE [dbo].[InvalidFiles]
(
	[InvalidFileId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FilePath] NVARCHAR(512) NOT NULL, 
    [FileTimestamp] DATETIME NOT NULL, 
	[ErrorMessage] NVARCHAR(MAX) NOT NULL,
    [ResolvedBy] NVARCHAR(50) NULL
)
