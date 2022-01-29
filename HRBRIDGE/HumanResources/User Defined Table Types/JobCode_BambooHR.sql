CREATE TYPE [hrbridge].[JobCode_BambooHR] AS TABLE
(
	[EmployeeId] INT,
	[UpdatedAt] DATETIME,
	[JobCodeEffectiveDate] DATETIME,
	[JobCode] VARCHAR(50),
	[UOfUJobTitle] VARCHAR(50)
)
