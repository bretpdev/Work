CREATE TABLE [dbo].[FCST_DAT_OnelinkQueues] (
    [ComplDate]    DATETIME    NULL,
    [Queue]        CHAR (10)   NULL,
    [User]         CHAR (7)    NULL,
    [ComTasks]     VARCHAR (7) NULL,
    [TotalHours]   VARCHAR (3) NULL,
    [TotalMinutes] VARCHAR (2) NULL,
    [TotalSeconds] VARCHAR (2) NULL,
    [AvgHours]     VARCHAR (2) NULL,
    [AvgMins]      VARCHAR (2) NULL,
    [AvgSec]       VARCHAR (2) NULL,
    [UserUD]       VARCHAR (2) NULL
);

