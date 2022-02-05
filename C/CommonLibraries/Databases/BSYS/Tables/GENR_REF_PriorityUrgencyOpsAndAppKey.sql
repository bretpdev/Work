CREATE TABLE [dbo].[GENR_REF_PriorityUrgencyOpsAndAppKey] (
    [UrgOption] VARCHAR (200) NOT NULL,
    [AppKey]    VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_GENR_REF_PriorityUrgencyOpsAndAppKey] PRIMARY KEY CLUSTERED ([AppKey] ASC, [UrgOption] ASC),
    CONSTRAINT [FK_GENR_REF_PriorityUrgencyOpsAndAppKey_GENR_REF_PriorityUrgencyOps] FOREIGN KEY ([UrgOption]) REFERENCES [dbo].[GENR_REF_PriorityUrgencyOps] ([UrgOption]) ON UPDATE CASCADE
);

