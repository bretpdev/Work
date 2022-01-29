CREATE TABLE [dbo].[SYSA_DAT_Users] (
    [SqlUserId]       INT           IDENTITY (1, 1) NOT NULL,
    [WindowsUserName] VARCHAR (50)  CONSTRAINT [DF_SYSA_DAT_Users_WindowsUserName] DEFAULT ('') NOT NULL,
    [FirstName]       VARCHAR (50)  CONSTRAINT [DF_SYSA_DAT_Users_FirstName] DEFAULT ('') NOT NULL,
    [MiddleInitial]   CHAR (1)      CONSTRAINT [DF_SYSA_DAT_Users_MiddleInitial] DEFAULT ('') NOT NULL,
    [LastName]        VARCHAR (50)  CONSTRAINT [DF_SYSA_DAT_Users_LastName] DEFAULT ('') NOT NULL,
    [EMail]           VARCHAR (100) CONSTRAINT [DF_SYSA_DAT_Users_EMail] DEFAULT ('') NOT NULL,
    [Extension]       VARCHAR (4)   CONSTRAINT [DF_SYSA_DAT_Users_Extension] DEFAULT ('') NOT NULL,
    [Extension2]      VARCHAR (4)   CONSTRAINT [DF_SYSA_DAT_Users_Extension2] DEFAULT ('') NOT NULL,
    [AesAccountId]    CHAR (10)     CONSTRAINT [DF_SYSA_DAT_Users_AesAccountId] DEFAULT ('') NOT NULL,
    [Title]           VARCHAR (100) CONSTRAINT [DF_SYSA_DAT_Users_Title] DEFAULT ('') NOT NULL,
    [Status]          VARCHAR (50)  CONSTRAINT [DF_SYSA_DAT_Users_Status] DEFAULT ('Active') NOT NULL,
    [BusinessUnit]    INT           NULL,
    [Role]            INT           NULL,
    [AesUserId]       VARCHAR (7)   NULL,
    CONSTRAINT [PK_SYSA_DAT_Users] PRIMARY KEY CLUSTERED ([SqlUserId] ASC)
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[SYSA_DAT_Users] TO [UHEAA\UHEAAUsers]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SYSA_DAT_Users] TO [UHEAA\CornerStoneUsers]
    AS [dbo];

