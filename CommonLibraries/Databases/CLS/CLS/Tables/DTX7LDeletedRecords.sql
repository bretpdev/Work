CREATE TABLE [dbo].[DTX7LDeletedRecords]
(
	[DTX7LDeletedRecordId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Ssn] CHAR(9) NOT NULL, 
    [Arc] VARCHAR(5) NOT NULL, 
    [RequestDate] DATETIME NOT NULL, 
    [LetterId] VARCHAR(10) NOT NULL, 
	[IsDueDiligence] BIT NOT NULL,
    [ProcessedAt] DATETIME NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE()
)
