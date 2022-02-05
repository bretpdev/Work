CREATE TABLE [dbo].[SYSA_REF_User_AppAndMod] (
    [WindowsUserName] NVARCHAR (50) NOT NULL,
    [AppOrModName]    VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_SYSA_REF_User_AppAndMod] PRIMARY KEY CLUSTERED ([WindowsUserName] ASC, [AppOrModName] ASC),
    CONSTRAINT [FK_SYSA_REF_User_AppAndMod_SYSA_LST_ApplicationsAndModules] FOREIGN KEY ([AppOrModName]) REFERENCES [dbo].[SYSA_LST_ApplicationsAndModules] ([Name]) ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_REF_User_AppAndMod_SYSA_LST_Users] FOREIGN KEY ([WindowsUserName]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON DELETE CASCADE ON UPDATE CASCADE
);

