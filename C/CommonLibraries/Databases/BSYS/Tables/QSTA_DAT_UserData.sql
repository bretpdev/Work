CREATE TABLE [dbo].[QSTA_DAT_UserData] (
    [RunTimeDate]   DATETIME      NOT NULL,
    [Queue]         NVARCHAR (8)  NOT NULL,
    [UserID]        NVARCHAR (7)  NOT NULL,
    [StatusCode]    NVARCHAR (50) NOT NULL,
    [CountInStatus] BIGINT        NOT NULL,
    [TotalTime]     NVARCHAR (50) NULL,
    [AvgTime]       NVARCHAR (50) NULL,
    CONSTRAINT [PK_QSTA_DAT_UserDataTEMP] PRIMARY KEY CLUSTERED ([RunTimeDate] ASC, [Queue] ASC, [UserID] ASC, [StatusCode] ASC),
    CONSTRAINT [FK_QSTA_DAT_UserData_QSTA_DAT_QueueData] FOREIGN KEY ([RunTimeDate], [Queue]) REFERENCES [dbo].[QSTA_DAT_QueueData] ([RunTimeDate], [Queue]),
    CONSTRAINT [FK_QSTA_DAT_UserData_SYSA_LST_UserIDInfo] FOREIGN KEY ([UserID]) REFERENCES [dbo].[SYSA_LST_UserIDInfo] ([UserID]) ON DELETE CASCADE ON UPDATE CASCADE
);

