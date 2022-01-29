CREATE TABLE [finalrev].[BorrowerRecord]
(
	[BorrowerRecordID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Ssn] VARCHAR(9) NOT NULL, 
    [Step] INT NOT NULL DEFAULT 0,
	[SchoolLetterNeeded] BIT NULL DEFAULT 0,
    [SchoolLetterSent] DATETIME NULL,
    [ProcessedAt] DATETIME NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)