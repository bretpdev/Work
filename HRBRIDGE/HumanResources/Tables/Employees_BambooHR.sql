CREATE TABLE [hrbridge].[Employees_BambooHR]
(
	[EmployeeId] INT NOT NULL PRIMARY KEY,
	[FirstName] VARCHAR(50) NULL,
	[LastName] VARCHAR(50) NULL,
	[WorkEmail] VARCHAR(200) NULL,
	[Department] VARCHAR(50) NULL,
	[Location] VARCHAR(50) NULL,
	[JobTitle] VARCHAR(50) NULL,
	[Supervisor] VARCHAR(200) NULL,
	[Division] VARCHAR(200) NULL,
	[UpdatedAt] DATETIME,
	[NeedsUpdated] BIT NOT NULL DEFAULT(0),
	[DeletedAt] DATETIME NULL
)
