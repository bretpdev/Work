CREATE TYPE [hrbridge].[CustomReport] AS TABLE
(
	[EmployeeId] INT NOT NULL,
	[FirstName] VARCHAR(500) NULL,
	[LastName] VARCHAR(500) NULL,
	[WorkEmail] VARCHAR(500) NULL,
	[Department] VARCHAR(500) NULL,
	[Location] VARCHAR(500) NULL,
	[JobTitle] VARCHAR(500) NULL,
	[ReportsTo] VARCHAR(500) NULL,
	[Division] VARCHAR(500) NULL,
	[HireDate] DATE NULL,
	[Ethnicity] VARCHAR(500),
	[EmployeeNumber] VARCHAR(500),
	[EEOJobCategory] VARCHAR(500),
	[Clearance] VARCHAR(500),
	[Birthdate] VARCHAR(500),
	[EmployeeLevel] VARCHAR(500),
	[Gender] VARCHAR(500),
	[SCACode] VARCHAR(500),
	[VacationCategory] VARCHAR(500),
	[DepartmentId] VARCHAR(500),
	[EffectiveDate] DATE
)
