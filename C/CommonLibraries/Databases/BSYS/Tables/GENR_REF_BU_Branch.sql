CREATE TABLE [dbo].[GENR_REF_BU_Branch] (
    [Branch]       NVARCHAR (50) NOT NULL,
    [BusinessUnit] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_REF_BU_Branch] PRIMARY KEY CLUSTERED ([Branch] ASC, [BusinessUnit] ASC),
    CONSTRAINT [FK_GENR_REF_BU_Branch_GENR_LST_BusinessUnits1] FOREIGN KEY ([BusinessUnit]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE
);

