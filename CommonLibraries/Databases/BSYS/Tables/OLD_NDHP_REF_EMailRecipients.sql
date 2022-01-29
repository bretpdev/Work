CREATE TABLE [dbo].[OLD_NDHP_REF_EMailRecipients] (
    [Ticket]          BIGINT        NOT NULL,
    [WindowsUserName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_NDHP_REF_EMailRecipients] PRIMARY KEY CLUSTERED ([WindowsUserName] ASC, [Ticket] ASC),
    CONSTRAINT [FK_NDHP_REF_EMailRecipients_NDHP_DAT_Tickets] FOREIGN KEY ([Ticket]) REFERENCES [dbo].[OLD_NDHP_DAT_Tickets] ([Ticket]) ON UPDATE CASCADE,
    CONSTRAINT [FK_NDHP_REF_EMailRecipients_SYSA_LST_Users] FOREIGN KEY ([WindowsUserName]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User''s Windows user name.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_EMailRecipients', @level2type = N'COLUMN', @level2name = N'WindowsUserName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique ticket number.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_EMailRecipients', @level2type = N'COLUMN', @level2name = N'Ticket';

