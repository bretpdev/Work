﻿CREATE TABLE [hrbridge].[EmployeeCustomReport]
(
	[EmployeeCustomReportId] INT NOT NULL IDENTITY PRIMARY KEY,
	[EmployeeId] INT NOT NULL,
	[FirstName] VARCHAR(500) NULL,
	[LastName] VARCHAR(500) NULL,
	[WorkEmail] VARCHAR(500) NULL,
	--[Department] VARCHAR(500) NULL,
	--[Location] VARCHAR(500) NULL,
	--[JobTitle] VARCHAR(500) NULL,
	--[ReportsTo] VARCHAR(500) NULL,
	--[Division] VARCHAR(500) NULL,
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
	[EffectiveDate] DATE,
	[UpdatedAt] DATETIME,
	[BirthdateRaw] VARCHAR(500) NULL
)