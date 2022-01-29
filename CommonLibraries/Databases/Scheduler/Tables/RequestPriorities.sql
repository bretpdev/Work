CREATE TABLE [dbo].[RequestPriorities] (
    [RequestPriorityId] INT IDENTITY (1, 1) NOT NULL,
    [ParentId]          INT NULL,
    [RequestTypeId]     INT NOT NULL,
    [RequestId]         INT NOT NULL,
    PRIMARY KEY CLUSTERED ([RequestPriorityId] ASC),
    CONSTRAINT [FK_RequestPriorities_RequestType] FOREIGN KEY ([RequestTypeId]) REFERENCES [dbo].[RequestTypes] ([RequestTypeId]),
    CONSTRAINT [FK_RequestPriorities_Self] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[RequestPriorities] ([RequestPriorityId]),
    CONSTRAINT [AK_RequestPriorities_PrioritizedAfter] UNIQUE NONCLUSTERED ([ParentId] ASC)
);



