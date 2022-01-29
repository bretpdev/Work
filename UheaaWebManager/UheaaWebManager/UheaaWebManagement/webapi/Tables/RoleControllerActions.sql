CREATE TABLE [webapi].[RoleControllerActions]
(
	[RoleControllerActionId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RoleId] INT NOT NULL, 
    [ControllerActionId] INT NOT NULL, 
	[AddedAt] DATETIME DEFAULT GETDATE() NOT NULL,
	[AddedBy] VARCHAR(100) NOT NULL,
	[InactivatedAt] DATETIME NULL,
	[InactivatedBy] VARCHAR(100) NULL, 
	CONSTRAINT [CK_RoleControllerActions_InactivatedAtInactivatedBy] CHECK ((InactivatedAt IS NULL AND InactivatedBy IS NULL) OR (InactivatedAt IS NOT NULL AND InactivatedBy IS NOT NULL)), 
    CONSTRAINT [FK_RoleControllerActions_Roles] FOREIGN KEY ([RoleId]) REFERENCES [webapi].[Roles]([RoleId]), 
    CONSTRAINT [FK_RoleControllerActions_ControllerActions] FOREIGN KEY ([ControllerActionId]) REFERENCES [webapi].[ControllerActions]([ControllerActionId])
)
