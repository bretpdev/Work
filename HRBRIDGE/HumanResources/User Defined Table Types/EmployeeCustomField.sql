CREATE TYPE [hrbridge].[EmployeeCustomField] AS TABLE
(
	[EmployeeId] INT NOT NULL,
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
	[DepartmentId] VARCHAR(500)
)
