CREATE TABLE [accurint].[ResponseFileProcessingQueue]
(
	[ResponseFileId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RunId] INT NOT NULL,
	[ResponseFileName] VARCHAR(260) NOT NULL,
	[ArchivedFileName] VARCHAR(260) NULL,
	[ProcessedAt] DATETIME,
	[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[DeletedAt] DATETIME,
	[DeletedBy] VARCHAR(250),
	CONSTRAINT [FK_ResponseFileProcessingQueue_RunLogger] FOREIGN KEY ([RunId]) REFERENCES [accurint].[RunLogger] ([RunId])
)
