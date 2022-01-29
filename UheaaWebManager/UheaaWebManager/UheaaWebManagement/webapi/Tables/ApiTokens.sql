CREATE TABLE [webapi].[ApiTokens]
(
	[ApiTokenId] INT NOT NULL PRIMARY KEY IDENTITY,
	[GeneratedToken] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	[Notes] VARCHAR(1000) NOT NULL,
	[StartDate] DATETIME NOT NULL DEFAULT GETDATE(),
	[EndDate] DATETIME,
	[AddedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[AddedBy] VARCHAR(100) NOT NULL ,
	[InactivatedAt] DATETIME NULL,
	[InactivatedBy] VARCHAR(100) NULL, 
    CONSTRAINT [CK_ApiTokens_InactivatedAtInactivatedBy] CHECK ((InactivatedAt IS NULL AND InactivatedBy IS NULL) OR (InactivatedAt IS NOT NULL AND InactivatedBy IS NOT NULL))
)
