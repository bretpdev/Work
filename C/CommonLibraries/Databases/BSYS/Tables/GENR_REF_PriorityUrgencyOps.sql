CREATE TABLE [dbo].[GENR_REF_PriorityUrgencyOps] (
    [UrgOption] VARCHAR (200) NOT NULL,
    [Urgency]   VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_GENR_REF_PriorityUrgencyOps] PRIMARY KEY CLUSTERED ([UrgOption] ASC),
    CONSTRAINT [FK_GENR_REF_PriorityUrgencyOps_GENR_LST_PriorityUrgencies] FOREIGN KEY ([Urgency]) REFERENCES [dbo].[GENR_LST_PriorityUrgencies] ([Urgency]) ON UPDATE CASCADE
);

