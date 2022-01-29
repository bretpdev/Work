CREATE TABLE [dbo].[LocateQueues] (
    [Queue]    CHAR (2) NOT NULL,
    [SubQueue] CHAR (2) NOT NULL,
    CONSTRAINT [PK_LocateQueues] PRIMARY KEY CLUSTERED ([Queue] ASC, [SubQueue] ASC)
);

