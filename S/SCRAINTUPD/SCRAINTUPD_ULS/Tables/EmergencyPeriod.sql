CREATE TABLE [scra].[EmergencyPeriod]
(
	[EmergencyPeriodId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] VARCHAR(200) NOT NULL,
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[CreatedAt] DATETIME DEFAULT(GETDATE()),
	[CreatedBy] VARCHAR(50) DEFAULT(SUSER_SNAME()),
	[DeletedAt] DATETIME,
	[DeletedBy] VARCHAR(50)
)