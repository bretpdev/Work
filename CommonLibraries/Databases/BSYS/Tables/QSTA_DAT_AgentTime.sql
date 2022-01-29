CREATE TABLE [dbo].[QSTA_DAT_AgentTime] (
    [RecNum]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [DateOfActivity] DATETIME     NOT NULL,
    [AgentID]        NVARCHAR (7) NULL,
    [OnLineTime]     BIGINT       NULL,
    [Connects]       INT          NULL,
    [ConnPerHr]      FLOAT (53)   NULL,
    [AvgTalkTime]    BIGINT       NULL,
    [AvgUpdateTime]  BIGINT       NULL,
    [AvgIdleTime]    BIGINT       NULL,
    CONSTRAINT [PK_QS_AgentTime] PRIMARY KEY CLUSTERED ([RecNum] ASC)
);

