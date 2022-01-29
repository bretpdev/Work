CREATE TABLE [dbo].[OLD_NDHP_LST_InUse] (
    [Ticket]   BIGINT        NOT NULL,
    [UserName] NVARCHAR (50) CONSTRAINT [DF_NDHP_LST_InUse_WindowsUserName] DEFAULT ('') NOT NULL,
    [Since]    DATETIME      CONSTRAINT [DF_NDHP_LST_InUse_Since] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_NDHP_LST_InUse] PRIMARY KEY CLUSTERED ([Ticket] ASC, [UserName] ASC),
    CONSTRAINT [FK_NDHP_LST_InUse_NDHP_DAT_Tickets] FOREIGN KEY ([Ticket]) REFERENCES [dbo].[OLD_NDHP_DAT_Tickets] ([Ticket]) ON UPDATE CASCADE,
    CONSTRAINT [FK_NDHP_LST_InUse_SYSA_LST_Users] FOREIGN KEY ([UserName]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User''s name.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_InUse', @level2type = N'COLUMN', @level2name = N'UserName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique ticket number.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_InUse', @level2type = N'COLUMN', @level2name = N'Ticket';

