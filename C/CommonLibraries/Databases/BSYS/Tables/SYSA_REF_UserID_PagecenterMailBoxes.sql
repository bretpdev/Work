CREATE TABLE [dbo].[SYSA_REF_UserID_PagecenterMailBoxes] (
    [UserID]  NVARCHAR (7)  NOT NULL,
    [Mailbox] NVARCHAR (15) NOT NULL,
    CONSTRAINT [PK_SYSA_REF_UserID_PagecenterMailBoxes] PRIMARY KEY CLUSTERED ([UserID] ASC, [Mailbox] ASC),
    CONSTRAINT [FK_SYSA_REF_UserID_PagecenterMailBoxes_SYSA_LST_PageCenterMailboxes] FOREIGN KEY ([Mailbox]) REFERENCES [dbo].[SYSA_LST_PageCenterMailboxes] ([Mailbox]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_REF_UserID_PagecenterMailBoxes_SYSA_LST_UserIDInfo] FOREIGN KEY ([UserID]) REFERENCES [dbo].[SYSA_LST_UserIDInfo] ([UserID]) ON DELETE CASCADE ON UPDATE CASCADE
);

