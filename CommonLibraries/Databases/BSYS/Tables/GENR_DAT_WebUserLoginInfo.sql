CREATE TABLE [dbo].[GENR_DAT_WebUserLoginInfo] (
    [UserName] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (25) NOT NULL,
    CONSTRAINT [PK_GENR_DAT_UserLoginInfo] PRIMARY KEY CLUSTERED ([UserName] ASC)
);

