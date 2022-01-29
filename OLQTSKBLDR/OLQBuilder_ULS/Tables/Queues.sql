CREATE TABLE [olqtskbldr].[Queues]
(
	[QueueId] INT NOT NULL PRIMARY KEY IDENTITY,
	[TargetId] VARCHAR(10),
	[QueueName] VARCHAR(10),
	[InstitutionId] VARCHAR(20),
	[InstitutionType] VARCHAR(20),
	[DateDue] DATE,
	[TimeDue] TIME,
	[Comment] VARCHAR(200),
	[SourceFilename] VARCHAR(200) NULL,
	[ProcessedAt] DATETIME,
	[AddedAt] DATETIME,
	[AddedBy] VARCHAR(30),
	[DeletedAt] DATETIME,
	[DeletedBy] VARCHAR(30),
	[ProcessingAttempts] INT DEFAULT(0)
)
