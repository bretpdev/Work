CREATE TABLE [dbo].[GENR_LST_PriorityEmailRecipient] (
    [SqlUserID] INT NOT NULL,
    [Priority]  INT NOT NULL,
    CONSTRAINT [PK_GENR_LST_PriorityEmailRecipient] PRIMARY KEY CLUSTERED ([SqlUserID] ASC, [Priority] ASC),
    CONSTRAINT [FK_GENR_LST_PriorityEmailRecipient_SYSA_DAT_Users] FOREIGN KEY ([SqlUserID]) REFERENCES [dbo].[SYSA_DAT_Users] ([SqlUserId])
);

