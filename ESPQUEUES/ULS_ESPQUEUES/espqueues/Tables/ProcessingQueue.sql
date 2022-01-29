CREATE TABLE [espqueues].[ProcessingQueue]
(
	[ProcessingQueueId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BorrowerSsn] CHAR(9) NOT NULL,
	[Queue] CHAR(2) NOT NULL,
	[SubQueue] CHAR(2) NOT NULL,
	[TaskControlNumber] VARCHAR(18) NOT NULL,
	[RequestArc] VARCHAR(5) NOT NULL,
	[RequestArcCreatedAt] DATETIME2(7) NOT NULL,
	[HasOtherGuarantor] BIT NOT NULL,
	[ArcAddProcessingId] BIGINT NULL, 
	[ProcessedAt] DATETIME NULL, 
	[ReassignedAt] DATETIME NULL,
	[ProcessingStepId] INT NULL,
	[AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(100) NULL
	CONSTRAINT [FK_ProcessingQueue_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [ArcAddProcessing]([ArcAddProcessingId]),
	CONSTRAINT [FK_ProcessingQueue_ProcessingStepId] FOREIGN KEY ([ProcessingStepId]) REFERENCES espqueues.ProcessingSteps([ProcessingStepId])
)
