CREATE TABLE [nsldsconso].[DataLoadRuns]
(
	[DataLoadRunId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StartedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [StartedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
	[BorrowerCount] INT NOT NULL,
	[Filename] VARCHAR(256) NOT NULL,
	[RecoveryMigratedTo] INT NULL,
    [EndedOn] DATETIME NULL, 
    CONSTRAINT [FK_BanaLoadRuns_RecoveryMigration] FOREIGN KEY ([DataLoadRunId]) REFERENCES [nsldsconso].[DataLoadRuns]([dataLoadRunId])
)