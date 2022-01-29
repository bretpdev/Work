CREATE TABLE [dasforbfed].[Disasters]
(
	[DisasterId] INT NOT NULL PRIMARY KEY,
	[Disaster] VARCHAR(100) NOT NULL,
	[BeginDate] DATETIME NOT NULL,
	[EndDate] DATETIME NOT NULL,
	[Days] AS DATEDIFF(DAY,BeginDate,EndDate)+1,
	[MaxEndDate] DATETIME NOT NULL,
	[MaxDays] AS DATEDIFF(DAY,BeginDate,MaxEndDate)+1,
	[Active] BIT NOT NULL,
	[AddedAt] DATETIME NOT NULL DEFAULT getdate(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT system_user, 
)
