CREATE TABLE [hrbridge].[EmploymentStatus]
(
	[EmploymentStatusId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[UpdatedAt] DATETIME NOT NULL,
	[Date] DATETIME NULL,
	[EmploymentStatus] VARCHAR(200),
	[EmploymentStatusComment] VARCHAR(1000),
	[TerminationReason] VARCHAR(100),
	[TerminationType] VARCHAR(100),
	[ElligableForRehire] VARCHAR(100),
)
