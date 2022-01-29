﻿CREATE TABLE [pifltr].[ProcessingQueue]
(
	[ProcessQueueId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Queue] VARCHAR(2) NOT NULL, 
    [SubQueue] VARCHAR(2) NOT NULL, 
    [TaskControlNumber] VARCHAR(18) NOT NULL, 
    [RequestArc] VARCHAR(5) NOT NULL, 
	[TaskRequestedDate] DATE NOT NULL,
	[Ssn] VARCHAR(9) NULL,
	[CoBorrowerSsn] VARCHAR(9) NULL ,
	[AccountNumber] varchar(10) NULL, 
	[LoanSeq] int NULL,
	[GuaranteedDate] date NULL,
	[GuaranteedAmount] numeric(8,2) null,
	[IsConsolPif] BIT NULL, 
	[IsCanceled] BIT NULL, 
    [ScriptDataId] INT NULL,
	[PrintProcessingId] BIGINT NULL, 
	[CoBwrPrintProcessingId] BIGINT NULL, 
    [QueueCompleterId] INT NULL, 
	[AddedAt] DATETIME NULL, 
    [AddedBy] VARCHAR(50) NULL, 
	[ProcessedAt] DATETIME NULL,
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
    
   
    
	
)
