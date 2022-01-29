CREATE TYPE [hrbridge].[CompensationRecord_BambooHR] AS TABLE
(
	[EmployeeId] INT NOT NULL,
	[UpdatedAt] DATETIME NULL,
	[StartDate] DATE NULL,
	[Rate] VARCHAR(20) NULL,
	[Type] VARCHAR(30) NULL,
	[Exempt] VARCHAR(10) NULL,
	[Reason] VARCHAR(100) NULL,
	[Comment] VARCHAR(500) NULL,
	[PaidPer] VARCHAR(30) NULL,
	[PaySchedule] VARCHAR(100) NULL
)
