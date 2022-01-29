CREATE TABLE [dbo].[SYSA_LST_OneLINKQueueGroups] (
    [Queue Groups]      NVARCHAR (3)  NOT NULL,
    [Group Description] NVARCHAR (50) NULL,
    CONSTRAINT [PK_SYSA_LST_OneLINKQueueGroups] PRIMARY KEY CLUSTERED ([Queue Groups] ASC)
);

