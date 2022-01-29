CREATE TABLE [dbo].[GENR_REF_SupportAssign] (
    [BusFunction] VARCHAR (100) NOT NULL,
    [Role]        VARCHAR (50)  NOT NULL,
    [Staff]       NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_COMN_REF_SupportAssign] PRIMARY KEY CLUSTERED ([BusFunction] ASC, [Role] ASC),
    CONSTRAINT [FK_COMN_REF_SupportAssign_COMN_LST_BusFunctionRolls] FOREIGN KEY ([Role]) REFERENCES [dbo].[GENR_LST_BusFunctionRoles] ([Role]) ON UPDATE CASCADE,
    CONSTRAINT [FK_COMN_REF_SupportAssign_COMN_LST_BusFunctions] FOREIGN KEY ([BusFunction]) REFERENCES [dbo].[GENR_LST_BusFunctions] ([BusFunction]) ON UPDATE CASCADE,
    CONSTRAINT [FK_COMN_REF_SupportAssign_SYSA_LST_Users] FOREIGN KEY ([Staff]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);

