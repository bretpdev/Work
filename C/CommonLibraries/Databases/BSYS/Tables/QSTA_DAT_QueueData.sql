CREATE TABLE [dbo].[QSTA_DAT_QueueData] (
    [RunTimeDate] DATETIME     NOT NULL,
    [Queue]       NVARCHAR (8) NOT NULL,
    [Total]       BIGINT       NOT NULL,
    [Complete]    BIGINT       NOT NULL,
    [Critical]    BIGINT       NOT NULL,
    [Cancelled]   BIGINT       NOT NULL,
    [Outstanding] BIGINT       NOT NULL,
    [Problem]     BIGINT       NOT NULL,
    [Late]        BIGINT       NULL,
    [Dept]        NVARCHAR (3) NULL,
    CONSTRAINT [PK_QS_QueueData] PRIMARY KEY CLUSTERED ([RunTimeDate] ASC, [Queue] ASC)
);

