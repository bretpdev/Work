CREATE TABLE [imgemailar].[ArchiveRequests]
(
	[ArchiveRequestId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ArcAddProcessingId] INT NULL,
	[AccountNumber] CHAR(10),
	[CreatedBy] VARCHAR(50),
	[CreatedAt] DATETIME DEFAULT GETDATE()
)
