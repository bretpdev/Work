CREATE TABLE [deskaudits].[RolesAndAccess]
(
	[RoleAccessId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RoleId] INT NOT NULL,
	[SearchAccess] BIT NOT NULL,
	[SubmitAccess] BIT NOT NULL,
	[CreatedAt] DATETIME DEFAULT(GETDATE()) NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(250) NULL
)
