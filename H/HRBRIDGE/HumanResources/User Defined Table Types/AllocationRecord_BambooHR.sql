CREATE TYPE [hrbridge].[AllocationRecord_BambooHR] AS TABLE
(
	[EmployeeId] INT,
	[UpdatedAt] DATETIME,
	[BusinessUnit] VARCHAR(500),
	[CostCenter] VARCHAR(500),
	[Account] VARCHAR(500),
	[FTE] VARCHAR(500),
	[AllocationEffectiveDate] DATETIME,
	[SquareFootage] INT
)
