CREATE TABLE [dbo].[R302_LST_Users] (
    [UserID]   NVARCHAR (7) NOT NULL,
    [AssignID] NVARCHAR (7) NOT NULL,
    CONSTRAINT [PK_R302_LST_Users] PRIMARY KEY CLUSTERED ([UserID] ASC, [AssignID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User ID to assign manual task to when R3 02 - Claim Overpayment Queue script is run by UserID.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'R302_LST_Users', @level2type = N'COLUMN', @level2name = N'AssignID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User ID authorized to run R3 02 - Claim Overpayment Queue script.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'R302_LST_Users', @level2type = N'COLUMN', @level2name = N'UserID';

