CREATE TABLE [lslettersu].[StoredProcedures]
(
	[StoredProceduresId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StoredProcedureName] VARCHAR(100) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
