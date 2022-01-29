CREATE TABLE [schrpt].[ReportTypes]
(
	[ReportTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StoredProcedureName] VARCHAR(50) NOT NULL,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL, 
    CONSTRAINT [CK_ReportTypes_Deleted] CHECK ((DeletedAt IS NULL AND DeletedBy IS NULL) OR (DeletedAt IS NOT NULL AND DeletedBy IS NOT NULL))
)
