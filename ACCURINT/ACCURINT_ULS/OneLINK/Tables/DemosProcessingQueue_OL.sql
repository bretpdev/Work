CREATE TABLE [accurint].[DemosProcessingQueue_OL]
(
	[DemosId] INT NOT NULL PRIMARY KEY IDENTITY,
	[AccountNumber] CHAR(10) NOT NULL,
	[WorkGroup] VARCHAR(8) NULL,
	[Department] VARCHAR(3) NULL,
	[TaskCreatedAt] DATETIME2(7),
	[SendToAccurint] BIT NULL, 
	[AddedToRequestFileAt] DATETIME NULL, --Account for request task put in file being sent to Accurint
	[TaskCompletedAt] DATETIME NULL, --Request task closed in session
	[RequestCommentAdded] BIT NULL, --Comment for request task
	[AddressTaskQueueId] INT NULL, --Task created from response file. Inserted into OLQTSKBLDR table
	[PhoneTaskQueueId] INT NULL, --Task created from response file. Inserted into OLQTSKBLDR table
	[RunId] INT NOT NULL,
	[CreatedAt] DATETIME DEFAULT GETDATE() NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(250) NULL,
	CONSTRAINT [FK_DemosProcessingQueueOL_RunLogger] FOREIGN KEY ([RunId]) REFERENCES [accurint].[RunLogger] ([RunId]),
)
