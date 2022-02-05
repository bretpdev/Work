CREATE TABLE [dbo].[QSTA_DAT_JobTime] (
    [RecNum]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [DateOfActivity] DATETIME      NULL,
    [JobName]        NVARCHAR (20) NULL,
    [CallsPlaced]    INT           NULL,
    [CallsAban]      INT           NULL,
    [AvgTalkTime]    BIGINT        NULL,
    [AvgUpdateTime]  BIGINT        NULL,
    [AvgIdleTime]    BIGINT        NULL,
    CONSTRAINT [PK_QS_JobTime] PRIMARY KEY CLUSTERED ([RecNum] ASC)
);

