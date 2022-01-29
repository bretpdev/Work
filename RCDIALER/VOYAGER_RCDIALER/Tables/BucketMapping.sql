CREATE TABLE [rcdialer].[BucketMapping]
(
	[BucketMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Bucket] INT NOT NULL, 
    [BucketBegin] INT NOT NULL, 
    [BucketEnd] INT NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [DeletedAt] DATETIME NULL
)
