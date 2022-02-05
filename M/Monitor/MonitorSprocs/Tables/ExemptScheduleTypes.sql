CREATE TABLE [monitor].[ExemptScheduleTypes]
(
	[ExemptScheduleTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ScheduleName] VARCHAR(40) NOT NULL, 
    [ScheduleCode] CHAR(2) NOT NULL, 
    [SchedulePlanName] VARCHAR(40) NOT NULL
)
