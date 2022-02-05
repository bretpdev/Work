CREATE TABLE [enrqtskllr].[ProcessingQueue]
(
	[ProcessingQueueId] INT NOT NULL IDENTITY PRIMARY KEY,
	[AccountNumber] VARCHAR(10),
	[QueueId] INT NOT NULL,
	[QueueTaskCreatedAt] DATETIME2(7) NOT NULL,
	[ProcessedAt] DATETIME NULL, 
	[ArcAddedAt] DATETIME NULL,
	[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [CreatedBy] VARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(100) NULL,
	CONSTRAINT [FK_ProcessingQueue_Queues] FOREIGN KEY (QueueId) REFERENCES enrqtskllr.Queues([QueueId]),
)
