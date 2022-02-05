CREATE TABLE [hrbridge].[Allocations_BambooHR]
(
	[AllocationId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[UpdatedAt] DATETIME NOT NULL,
	[BusinessUnit] VARCHAR(500) NULL,
	[CostCenter] VARCHAR(500) NULL,
	[Account] VARCHAR(500) NULL,
	[FTE] decimal NULL,
	[AllocationEffectiveDate] DATETIME NULL,
	[SquareFootage] INT NULL,
	[FTERaw] VARCHAR(500)
)
