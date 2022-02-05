CREATE TABLE [accurint].[DemosProcessingQueue_UH]
(
	[DemosId] INT NOT NULL PRIMARY KEY IDENTITY,
	[AccountNumber] CHAR(10) NOT NULL,
	[EndorserSsn] CHAR(9) NULL,
	[Queue] VARCHAR(2) NULL,
	[SubQueue] VARCHAR(2) NULL,
	[TaskControlNumber] VARCHAR(18) NULL,
	[TaskRequestArc] VARCHAR(5) NULL,
	[TaskCreatedAt] DATETIME2(7),
	[AddedToRequestFileAt] DATETIME NULL, --Account for request task put in file being sent to Accurint
	[TaskCompletedAt] DATETIME NULL, --Request task closed in session
	[RequestArcId] BIGINT NULL, --Comment for request task
	[ResponseAddressArcId] BIGINT NULL, --ARC for accounts in response file
	[ResponsePhoneArcId] BIGINT NULL, --ARC for accounts in response file
	[RunId] INT NOT NULL,
	[CreatedAt] DATETIME DEFAULT GETDATE() NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(250) NULL,
	CONSTRAINT [FK_DemosProcessingQueueUH_RunLogger] FOREIGN KEY ([RunId]) REFERENCES [accurint].[RunLogger] ([RunId]),
)
