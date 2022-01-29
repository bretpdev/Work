CREATE TABLE [dbo].[GENR_REF_BU_Agent_Xref] (
    [BusinessUnit]  NVARCHAR (50) NOT NULL,
    [WindowsUserID] NVARCHAR (50) NOT NULL,
    [Role]          NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_COMN_REF_BU_Agent_Xref] PRIMARY KEY CLUSTERED ([BusinessUnit] ASC, [WindowsUserID] ASC, [Role] ASC),
    CONSTRAINT [FK_COMN_REF_BU_Agent_Xref_COMN_LST_Roles] FOREIGN KEY ([Role]) REFERENCES [dbo].[GENR_LST_Roles] ([Role]) ON UPDATE CASCADE,
    CONSTRAINT [FK_COMN_REF_BU_Agent_Xref_GENR_LST_BusinessUnits] FOREIGN KEY ([BusinessUnit]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE,
    CONSTRAINT [FK_COMN_REF_BU_Agent_Xref_SYSA_LST_Users] FOREIGN KEY ([WindowsUserID]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);

