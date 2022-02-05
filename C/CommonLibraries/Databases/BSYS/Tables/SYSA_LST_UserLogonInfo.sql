CREATE TABLE [dbo].[SYSA_LST_UserLogonInfo] (
    [WindowsUserID]  VARCHAR (50) NOT NULL,
    [FavoriteScreen] VARCHAR (10) NULL,
    [LogonRegion]    VARCHAR (50) NULL,
    CONSTRAINT [PK_SYSA_LST_UserLogonInfo] PRIMARY KEY CLUSTERED ([WindowsUserID] ASC)
);

