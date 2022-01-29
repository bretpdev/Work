CREATE TABLE [webapi].[UserTokens]
(
	[UserTokenId] INT NOT NULL PRIMARY KEY IDENTITY,
	[GeneratedToken] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	[AssociatedWindowsUsername] VARCHAR(100) NOT NULL DEFAULT REPLACE(SYSTEM_USER, 'UHEAA\', ''),
	[AddedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[TokenExpiresAt] DATETIME NOT NULL DEFAULT DATEADD(minute, 15, GETDATE()),
    CONSTRAINT [CK_UserTokens_AssociatedWindowsUsername_NoDomain] CHECK (AssociatedWindowsUsername NOT LIKE '%\%')
)
