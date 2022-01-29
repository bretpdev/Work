CREATE TABLE [dbo].[LST_InUse] (
    [Ticket]    BIGINT   NOT NULL,
    [SqlUserId] INT      CONSTRAINT [DF_NDHP_LST_InUse_WindowsUserName] DEFAULT ('') NOT NULL,
    [Since]     DATETIME CONSTRAINT [DF_NDHP_LST_InUse_Since] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_NDHP_LST_InUse] PRIMARY KEY CLUSTERED ([Ticket] ASC, [SqlUserId] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User''s name.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LST_InUse', @level2type = N'COLUMN', @level2name = N'SqlUserId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique ticket number.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LST_InUse', @level2type = N'COLUMN', @level2name = N'Ticket';

