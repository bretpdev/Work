CREATE TABLE [webapi].[ApiTokenControllerActions]
(
	[ApiTokenControllerActionId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ApiTokenId] INT NOT NULL, 
    [ControllerActionId] INT NOT NULL, 
	[AddedAt] DATETIME DEFAULT GETDATE() NOT NULL,
	[AddedBy] VARCHAR(100) NOT NULL,
	[InactivatedAt] DATETIME NULL,
	[InactivatedBy] VARCHAR(100) NULL, 
	CONSTRAINT [CK_ApiTokenControllerActions_InactivatedAtInactivatedBy] CHECK ((InactivatedAt IS NULL AND InactivatedBy IS NULL) OR (InactivatedAt IS NOT NULL AND InactivatedBy IS NOT NULL)), 
    CONSTRAINT [FK_ApiTokenControllerActions_ApiTokens] FOREIGN KEY ([ApiTokenId]) REFERENCES [webapi].[ApiTokens]([ApiTokenId]), 
    CONSTRAINT [FK_ApiTokenControllerActions_ControllerActions] FOREIGN KEY ([ControllerActionId]) REFERENCES [webapi].[ControllerActions]([ControllerActionId])

)
