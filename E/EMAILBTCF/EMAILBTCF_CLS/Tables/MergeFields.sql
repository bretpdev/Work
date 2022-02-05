CREATE TABLE [emailbtcf].[MergeFields]
(
	[MergeFieldId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MergeFieldName] VARCHAR(50) NOT NULL, 
    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [CreatedBy] VARCHAR(50) NOT NULL, 
    [UpdatedAt] DATETIME NULL, 
    [UpdatedBy] VARCHAR(50) NULL, 
    [Active] BIT NOT NULL
)
