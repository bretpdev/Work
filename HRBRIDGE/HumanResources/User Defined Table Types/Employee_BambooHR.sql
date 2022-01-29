CREATE TYPE [hrbridge].[Employee_BambooHR] AS TABLE
(
	[EmployeeId] INT NOT NULL,
	[FirstName] VARCHAR(500) NULL,
	[LastName] VARCHAR(500) NULL,
	[WorkEmail] VARCHAR(500) NULL,
	[Department] VARCHAR(500) NULL,
	[Location] VARCHAR(500) NULL,
	[JobTitle] VARCHAR(500) NULL,
	[Supervisor] VARCHAR(500) NULL,
	[Division] VARCHAR(500) NULL
)
