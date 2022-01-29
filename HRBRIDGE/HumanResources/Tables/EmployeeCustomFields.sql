CREATE TABLE [hrbridge].[EmployeeCustomFields]
(
	[EmployeeCustomFieldId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[HireDate] DATE NULL,
	[Ethnicity] VARCHAR(500),
	[EmployeeNumber] VARCHAR(500),
	[EEOJobCategory] VARCHAR(500),
	[Clearance] VARCHAR(500),
	[Birthdate] DATE,
	[EmployeeLevel] VARCHAR(500),
	[Gender] VARCHAR(500),
	[SCACode] VARCHAR(500),
	[VacationCategory] VARCHAR(500),
	[DepartmentId] VARCHAR(500),
	[NewHire] VARCHAR(500),
	[UpdatedAt] DATETIME,
	[NeedsUpdated] BIT,
	[HireDateRaw] VARCHAR(500) NULL,
	[BirthdateRaw] VARCHAR(500) NULL
)
