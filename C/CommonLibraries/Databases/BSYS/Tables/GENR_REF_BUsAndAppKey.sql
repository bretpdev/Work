CREATE TABLE [dbo].[GENR_REF_BUsAndAppKey] (
    [BusinessUnit]   NVARCHAR (50) NOT NULL,
    [ApplicationKey] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_REF_BUsAndAppKey] PRIMARY KEY CLUSTERED ([BusinessUnit] ASC, [ApplicationKey] ASC),
    CONSTRAINT [FK_GENR_REF_BUsAndAppKey_GENR_LST_BusinessUnits] FOREIGN KEY ([BusinessUnit]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE
);

