CREATE TABLE [webapi].[Roles]
(
	[RoleId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ActiveDirectoryRoleName] VARCHAR(50) NOT NULL,
	[Notes] VARCHAR(1000) NULL,
	[AddedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[AddedBy] VARCHAR(100) NOT NULL,
	[InactivatedAt] DATETIME NULL,
	[InactivatedBy] VARCHAR(100) NULL, 
	CONSTRAINT [CK_Roles_InactivatedAtInactivatedBy] CHECK ((InactivatedAt IS NULL AND InactivatedBy IS NULL) OR (InactivatedAt IS NOT NULL AND InactivatedBy IS NOT NULL))
)
