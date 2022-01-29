CREATE TABLE [dbo].[SYSA_DAT_RoleKeyAssignment] (
    [RoleID]    INT      NOT NULL,
    [UserKeyID] INT      NOT NULL,
    [AddedBy]   INT      NOT NULL,
    [StartDate] DATETIME NOT NULL,
    [RemovedBy] INT      NULL,
    [EndDate]   DATETIME NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'All the keys that are assigned to a role with the date/time it was added/removed and who add/removed the access', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSA_DAT_RoleKeyAssignment', @level2type = N'COLUMN', @level2name = N'RoleID';

