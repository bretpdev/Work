CREATE TABLE [mssasgndft].[Queues] (
    [QueueId]     INT          IDENTITY (1, 1) NOT NULL,
    [QueueName]   VARCHAR (8)  NOT NULL,
    [AddedOn]     DATETIME     DEFAULT (getdate()) NOT NULL,
    [AddedBy]     VARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]   DATETIME     NULL,
    [DeletedBy]   VARCHAR (50) NULL,
    [FutureDated] BIT          NULL,
    PRIMARY KEY CLUSTERED ([QueueId] ASC) WITH (FILLFACTOR = 95)
);

