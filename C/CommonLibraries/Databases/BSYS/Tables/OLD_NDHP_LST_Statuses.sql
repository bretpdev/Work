CREATE TABLE [dbo].[OLD_NDHP_LST_Statuses] (
    [Status] VARCHAR (50) CONSTRAINT [DF_NDHP_LST_Statuses_Status] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_NDHP_LST_Statuses] PRIMARY KEY CLUSTERED ([Status] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of ticket status.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_Statuses', @level2type = N'COLUMN', @level2name = N'Status';

