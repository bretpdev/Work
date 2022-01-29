CREATE TABLE [dbo].[OLD_NDHP_REF_Actions] (
    [WindowsUserName] NVARCHAR (50) NOT NULL,
    [HPAction]        NVARCHAR (50) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description of the action to take or permit for the user.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_Actions', @level2type = N'COLUMN', @level2name = N'HPAction';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User''s Windows user name.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_REF_Actions', @level2type = N'COLUMN', @level2name = N'WindowsUserName';

