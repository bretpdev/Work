CREATE TYPE [hrbridge].[EmploymentStatus_BambooHR] AS TABLE
(
	[EmployeeId] INT NOT NULL,
	[UpdatedAt] DATETIME NOT NULL,
	[Date] DATETIME NULL,
	[EmploymentStatus] VARCHAR(200),
	[EmploymentStatusComment] VARCHAR(1000),
	[TerminationReason] VARCHAR(100),
	[TerminationType] VARCHAR(100),
	[ElligableForRehire] VARCHAR(100)
)
