CREATE TABLE [dbo].[ScheduledHours] (
    [ScheduledHoursId] INT            IDENTITY (1, 1) NOT NULL,
    [DevHours]         DECIMAL (6, 2) NULL,
    [TestHours]        DECIMAL (6, 2) NULL,
    PRIMARY KEY CLUSTERED ([ScheduledHoursId] ASC)
);

