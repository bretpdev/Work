CREATE TABLE [webapps].[WebApps]
(
	[WebAppId] INT NOT NULL PRIMARY KEY IDENTITY,
	[AppName] VARCHAR(50) NOT NULL,
	[Url] VARCHAR(2048) NOT NULL,
	[IconBytes] VARBINARY NULL,
	[AddedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[AddedBy] VARCHAR(50) NOT NULL,
	[InactivatedAt] DATETIME NULL,
	[InactivatedBy] VARCHAR(100) NULL, 
    CONSTRAINT [CK_WebApps_InactivatedAtInactivatedBy] CHECK ((InactivatedAt IS NULL AND InactivatedBy IS NULL) OR (InactivatedAt IS NOT NULL AND InactivatedBy IS NOT NULL))
)
