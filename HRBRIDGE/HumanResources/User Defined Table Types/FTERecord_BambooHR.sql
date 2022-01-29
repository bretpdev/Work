CREATE TYPE [hrbridge].[FTERecord_BambooHR] AS TABLE
(
	[EmployeeId] INT,
	[UpdatedAt] DATETIME,
	[FTEEffectiveDate] DATETIME,
	[FTE] VARCHAR(50),
	[Notes] VARCHAR(500)
)
