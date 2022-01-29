CREATE TABLE [dbo].[GENR_LST_Priorities] (
    [Urgency]  VARCHAR (50) NOT NULL,
    [Category] VARCHAR (50) NOT NULL,
    [Priority] SMALLINT     NOT NULL,
    CONSTRAINT [PK_GENR_LST_Priorities] PRIMARY KEY CLUSTERED ([Urgency] ASC, [Category] ASC),
    CONSTRAINT [FK_GENR_LST_Priorities_GENR_LST_PriorityCatgrys] FOREIGN KEY ([Category]) REFERENCES [dbo].[GENR_LST_PriorityCatgrys] ([Category]) ON UPDATE CASCADE,
    CONSTRAINT [FK_GENR_LST_Priorities_GENR_LST_PriorityUrgencies] FOREIGN KEY ([Urgency]) REFERENCES [dbo].[GENR_LST_PriorityUrgencies] ([Urgency]) ON UPDATE CASCADE
);

