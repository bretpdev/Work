CREATE TABLE [accurint].[RunLogger]
(
	[RunId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RequestFileName] VARCHAR(260),
	[RequestFileCreatedAt] DATETIME,
	[RequestFileUploadedAt] DATETIME,
	[RecordsSent] INT,
	[ResponseFilesDownloadedAt] DATETIME,
	[RecordsReceived] INT,
	[ResponseFilesProcessedAt] DATETIME,
	[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[DeletedAt] DATETIME,
	[DeletedBy] VARCHAR(250),
)
