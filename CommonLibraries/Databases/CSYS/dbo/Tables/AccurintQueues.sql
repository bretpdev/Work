CREATE TABLE [dbo].[AccurintQueues] (
    [AccurintQueueId] INT          IDENTITY (1, 1) NOT NULL,
    [Queue]           VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_AccurintQueues] PRIMARY KEY CLUSTERED ([AccurintQueueId] ASC)
);

