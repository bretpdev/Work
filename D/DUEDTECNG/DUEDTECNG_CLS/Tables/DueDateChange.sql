CREATE TABLE [duedtecng].[DueDateChange]
(
	[DueDateChangeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Ssn] CHAR(9) NOT NULL, 
    [AccountNumber] CHAR(10) NOT NULL, 
    [DueDate] TINYINT NOT NULL, 
    [ProcessedAt] DATETIME NULL, 
    [ProcessedBy] VARCHAR(100) NULL, 
	[ManualReviewNeeded] BIT NULL,
    [AddedAt] DATETIME NOT NULL DEFAULT getdate(), 
    [AddedBy] VARCHAR(100) NOT NULL DEFAULT SYSTEM_USER, 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(100) NULL, 
    CONSTRAINT [CK_ProcessedAt_ProcessedBy_ManualReview] CHECK ((ProcessedAt IS NULL AND ProcessedBy IS NULL AND ManualReviewNeeded IS NULL) OR (ProcessedAt IS NOT NULL AND ProcessedBy IS NOT NULL AND ManualReviewNeeded IS NOT NULL)),
	CONSTRAINT [CK_DeletedAt_DeletedBy] CHECK ((DeletedAt IS NULL AND DeletedBy IS NULL) OR (DeletedAt IS NOT NULL AND DeletedBy IS NOT NULL)),
)
