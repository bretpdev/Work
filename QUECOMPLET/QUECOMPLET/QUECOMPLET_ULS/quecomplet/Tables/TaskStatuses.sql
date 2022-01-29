CREATE TABLE [quecomplet].[TaskStatuses] (
    [TaskStatusId] INT      IDENTITY (1, 1) NOT NULL,
    [TaskStatus]   CHAR (1) NOT NULL,
    PRIMARY KEY CLUSTERED ([TaskStatusId] ASC) WITH (FILLFACTOR = 95)
);

