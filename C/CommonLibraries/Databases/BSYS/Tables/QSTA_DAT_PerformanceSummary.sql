CREATE TABLE [dbo].[QSTA_DAT_PerformanceSummary] (
    [RecNum]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [DateOfActivity] DATETIME      NULL,
    [JobName]        NVARCHAR (20) NULL,
    [FirstStart]     NVARCHAR (8)  NULL,
    [LastEnd]        NVARCHAR (8)  NULL,
    [OnlineTime]     BIGINT        NULL,
    [CallsOffered]   INT           NULL,
    [Connects]       INT           NULL,
    [ConnPerHr]      FLOAT (53)    NULL,
    [CallsToQueue]   INT           NULL,
    [AvgQueueTime]   BIGINT        NULL,
    [AbandonRate]    FLOAT (53)    NULL,
    [ManualCalls]    INT           NULL,
    [ManualCallRate] FLOAT (53)    NULL,
    CONSTRAINT [PK_QS_PerformanceSummary] PRIMARY KEY CLUSTERED ([RecNum] ASC)
);

