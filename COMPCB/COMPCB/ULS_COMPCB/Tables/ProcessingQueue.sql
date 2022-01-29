CREATE TABLE [compcb].[ProcessingQueue]
(
	[ProcessingQueueId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [BorrowerSsn] CHAR(9) NOT NULL, 
    [BorrowerAccountNumber] CHAR(10) NOT NULL,
    [EndorserSsn] CHAR(9) NULL, 
    [EndorserAccountNumber] CHAR(10) NULL,
    [TaskControlNumber] VARCHAR(18) NOT NULL,
    [RequestArc] VARCHAR(5) NOT NULL,
    [TaskRequestedDate] DATE NOT NULL,
    [IsEndorserTask] BIT NULL, 
    [IsForeignAddress] BIT NULL, 
    [ProcessedAt] DATETIME NULL, 
    [ArcAddProcessingId] BIGINT NULL, 
    [ProcessingAttempts] INT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME NULL DEFAULT GETDATE(), 
    [CreatedBy] VARCHAR(100) NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(100) NULL
)
