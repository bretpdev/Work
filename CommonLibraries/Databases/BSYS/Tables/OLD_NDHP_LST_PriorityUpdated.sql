CREATE TABLE [dbo].[OLD_NDHP_LST_PriorityUpdated] (
    [PriorityUpdated] DATETIME NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date priorities were last updated.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_PriorityUpdated', @level2type = N'COLUMN', @level2name = N'PriorityUpdated';

