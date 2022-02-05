CREATE TABLE [dpapost].[PostingData]
(
	[PostingDataId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccountNumber] VARCHAR(10) NOT NULL, 
    [Amount] FLOAT NOT NULL, 
    [ProcessedAt] DATETIME NULL,
    [ErrorPosting] BIT NULL DEFAULT 0,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT USER_NAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
