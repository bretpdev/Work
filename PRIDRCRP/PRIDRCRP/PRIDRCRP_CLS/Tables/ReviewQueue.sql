CREATE TABLE [pridrcrp].[ReviewQueue]
(
	[ReviewQueueId] INT NOT NULL PRIMARY KEY IDENTITY,
	[SSN] VARCHAR(9) NOT NULL,
	[ArcAddProcessingId] BIGINT NULL,
	[ExceptionLog] VARCHAR(MAX) NULL,
	[CreatedAt] DATETIME NULL,
	[ReviewDate] DATETIME NULL,
	[Reviewer] VARCHAR(100) NULL,
	[ReviewComment] VARCHAR(MAX) NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(100) NULL
)
