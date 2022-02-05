CREATE TABLE [dbo].[REF_EMailRecipient] (
    [Ticket]    BIGINT NOT NULL,
    [SqlUserId] INT    NOT NULL,
    CONSTRAINT [PK_NDHP_REF_EMailRecipients] PRIMARY KEY CLUSTERED ([SqlUserId] ASC, [Ticket] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User''s Windows user name.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'REF_EMailRecipient', @level2type = N'COLUMN', @level2name = N'SqlUserId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique ticket number.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'REF_EMailRecipient', @level2type = N'COLUMN', @level2name = N'Ticket';

