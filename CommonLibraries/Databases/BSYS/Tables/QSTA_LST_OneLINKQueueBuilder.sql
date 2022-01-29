CREATE TABLE [dbo].[QSTA_LST_OneLINKQueueBuilder] (
    [Dept]            NVARCHAR (3)  NOT NULL,
    [ResultCode]      NVARCHAR (5)  NOT NULL,
    [QueueToPopulate] NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_QS_OneLINKQueueBuilder] PRIMARY KEY CLUSTERED ([Dept] ASC, [ResultCode] ASC)
);

