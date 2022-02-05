CREATE TABLE [projectrequest].[Roles]
(
	[RoleId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Role] VARCHAR(100) NOT NULL,
	[PermissionId] INT NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL
	CONSTRAINT [FK_Roles_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES projectrequest.[Permissions]([PermissionId])
)
