CREATE TABLE [hrbridge].[Compensation_BambooHR]
(
	[CompensationId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[UpdatedAt] DATETIME NULL,
	[StartDate] DATE NULL,
	[Rate] MONEY NULL,
	[Type] VARCHAR(500) NULL,
	[Exempt] VARCHAR(500) NULL,
	[Reason] VARCHAR(500) NULL,
	[Comment] VARCHAR(500) NULL,
	[PaidPer] VARCHAR(500) NULL,
	[PaySchedule] VARCHAR(500) NULL,
	[NeedsUpdated] BIT NOT NULL DEFAULT(0),
	[RateRaw] VARCHAR(500) NULL,
)
