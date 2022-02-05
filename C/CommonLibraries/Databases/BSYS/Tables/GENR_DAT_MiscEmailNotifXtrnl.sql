CREATE TABLE [dbo].[GENR_DAT_MiscEmailNotifXtrnl] (
    [TypeKey]      VARCHAR (300) NOT NULL,
    [EmailAddress] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_GENR_DAT_MiscEmailNitifXtrnl] PRIMARY KEY CLUSTERED ([TypeKey] ASC, [EmailAddress] ASC)
);

