CREATE TABLE [hrbridge].[JobCodes_BambooHR]
(
	[JobCodeId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[UpdatedAt] DATETIME NOT NULL,
	[JobCodeEffectiveDate] DATETIME NULL,
	[JobCode] VARCHAR(50) NULL,
	[UOfUJobTitle] VARCHAR(50) NULL
)
