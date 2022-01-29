CREATE TABLE [webapi].[UserTokenRoles]
(
	[UserTokenRoleId] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserTokenId] INT NOT NULL,
	[RoleId] INT NOT NULL
)
