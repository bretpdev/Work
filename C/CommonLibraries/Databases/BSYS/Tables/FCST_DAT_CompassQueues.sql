CREATE TABLE [dbo].[FCST_DAT_CompassQueues] (
    [Queue]         CHAR (2)     NULL,
    [Subqueue]      CHAR (2)     NULL,
    [ControlNumber] VARCHAR (50) NULL,
    [ARC]           CHAR (5)     NULL,
    [User]          CHAR (7)     NULL,
    [TaskTime]      FLOAT (53)   NULL,
    [ComplDate]     DATETIME     NULL,
    [UserID]        VARCHAR (2)  NULL
);

