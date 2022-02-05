CREATE TABLE [acurintc].[SystemSources]
(
	[SystemSourceId] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL,
	[LocateType] CHAR(3) NOT NULL, 
    [OneLinkSourceCode] CHAR NOT NULL, 
    [CompassSourceCode] CHAR(2) NOT NULL, 
    [ActivityType] CHAR(2) NOT NULL, 
    [ContactType] CHAR(2) NOT NULL
)
