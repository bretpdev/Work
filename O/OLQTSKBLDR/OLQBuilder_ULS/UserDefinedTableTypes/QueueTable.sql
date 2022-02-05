CREATE TYPE [olqtskbldr].[QueueTable] AS TABLE
(
	[TargetId] VARCHAR(10),
	[QueueName] VARCHAR(10),
	[InstitutionId] VARCHAR(20),
	[InstitutionType] VARCHAR(20),
	[DateDue] DATE,
	[TimeDue] TIME,
	[Comment] VARCHAR(200)
)
