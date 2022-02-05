CREATE TABLE [dbo].[GENR_LST_BusinessUnitEmailAddrs] (
    [BusinessUnit]        NVARCHAR (50)  NOT NULL,
    [AssociatedEmailAddr] NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_GENR_LST_BusinessUnitEmailAddrs] PRIMARY KEY CLUSTERED ([BusinessUnit] ASC),
    CONSTRAINT [FK_GENR_LST_BusinessUnitEmailAddrs_GENR_LST_BusinessUnits] FOREIGN KEY ([BusinessUnit]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE
);

