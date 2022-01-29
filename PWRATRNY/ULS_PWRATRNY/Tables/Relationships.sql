CREATE TABLE [pwratrny].[Relationships]
(
	[RelationshipsId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Description] VARCHAR(30) NOT NULL, 
    [CompassCode] VARCHAR(2) NOT NULL, 
    [OnelinkCode] VARCHAR(2) NOT NULL,
    [Active] BIT NOT NULL DEFAULT 1,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT USER_NAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
