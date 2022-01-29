CREATE TABLE [duplrefs].[BorrowerQueue_Retired]
(
	[BorrowerQueueId] INT NOT NULL IDENTITY PRIMARY KEY,
	[AccountNumber] CHAR(10) NOT NULL, 
	[UserId] VARCHAR(8) NULL,
	[ArcAddProcessingId] BIGINT NULL,
	[ProcessedAt] DATETIME NULL, 
	[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [CreatedBy] VARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(100) NULL
)
