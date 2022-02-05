CREATE TABLE [quecomplet].[Queues]
(
	[QueueId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Queue] VARCHAR(2) NOT NULL, 
    [SubQueue] VARCHAR(2) NOT NULL, 
    [TaskControlNumber] VARCHAR(20) NOT NULL, 
    [AccountIdentifier] VARCHAR(10) NOT NULL, 
    [TaskStatusId] INT NOT NULL, 
    [ActionResponseId] INT NULL, 
	[PickedUpForProcessing] DATETIME NULL,
	[ProcessedAt] DATETIME NULL,
	[HadError] BIT NOT NULL DEFAULT 0,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(100) NOT NULL, 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(100) NULL, 
    CONSTRAINT [FK_Queues_ToTaskStatuses] FOREIGN KEY (TaskStatusId) REFERENCES [quecomplet].[TaskStatuses](TaskStatusId), 
    CONSTRAINT [FK_Queues_ToActionResponses] FOREIGN KEY (ActionResponseId) REFERENCES [quecomplet].[ActionResponses](ActionResponseId) 

)