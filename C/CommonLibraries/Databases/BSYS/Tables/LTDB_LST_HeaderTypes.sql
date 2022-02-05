CREATE TABLE [dbo].[LTDB_LST_HeaderTypes]
(
	[HeaderTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HeaderType] VARCHAR(50) NOT NULL, 
    [CreatedBy] VARCHAR(50) NOT NULL, 
    [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [UpdatedBy] VARCHAR(50) NULL, 
    [UpdatedAt] DATETIME NULL, 
    [Active] BIT NULL DEFAULT 1
)
