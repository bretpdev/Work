CREATE TABLE [monitor].[ExemptScheduleTypes] (
    [ExemptScheduleTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [ScheduleName]         VARCHAR (40) NOT NULL,
    [ScheduleCode]         CHAR (2)     NOT NULL,
    [SchedulePlanName]     VARCHAR (40) NOT NULL,
    PRIMARY KEY CLUSTERED ([ExemptScheduleTypeId] ASC)
);

