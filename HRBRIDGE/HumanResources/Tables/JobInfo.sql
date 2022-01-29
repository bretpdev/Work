CREATE TABLE [hrbridge].[JobInfo]
(
	[JobInfoId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[UpdatedAt] DATETIME NOT NULL,
	[Date] DATETIME,
	[Location] VARCHAR(200),
	[Department] VARCHAR(200),
	[Division] VARCHAR(200),
	[JobTitle] VARCHAR(200),
	[ReportsTo] VARCHAR(200)
)
