CREATE TABLE [qworkerlgp].[Queues]
(
	[QueueId] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Ssn] VARCHAR(9),
	[Department] VARCHAR(10),
	[WorkGroupId] VARCHAR(10),	
	[ActionCode] VARCHAR(10),
	[ActivityType] VARCHAR(10),
	[ActivityContactType] VARCHAR(10),
	[TaskComment] VARCHAR(MAX),
	[HadError] BIT,
	[PickedUpForProcessing] DATETIME,
	[WasFound] BIT,
	[ProcessedAt] DATETIME,
	[DeletedAt] DATETIME,
	[DeletedBy] VARCHAR(100),
	[AddedAt] DATETIME,
	[AddedBy] VARCHAR(100)
)
