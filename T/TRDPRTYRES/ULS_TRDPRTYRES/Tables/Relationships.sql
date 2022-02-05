CREATE TABLE [trdprtyres].[Relationships]
(
	[RelationshipsId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Relationship] VARCHAR(50) NOT NULL, 
    [RelationshipCode] VARCHAR(2) NOT NULL, 
    [IsOnelink] BIT NOT NULL DEFAULT 0, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
