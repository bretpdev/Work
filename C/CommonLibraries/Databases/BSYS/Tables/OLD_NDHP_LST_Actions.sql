CREATE TABLE [dbo].[OLD_NDHP_LST_Actions] (
    [HPAction] NVARCHAR (50) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description of the action to take or permit for the user.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_Actions', @level2type = N'COLUMN', @level2name = N'HPAction';

