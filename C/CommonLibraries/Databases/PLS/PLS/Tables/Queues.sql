CREATE TABLE [crpqassign].[Queues]
(
	[QueueId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Queue] VARCHAR(2) NOT NULL, 
    [SubQueue] VARCHAR(2) NOT NULL, 
    [Arc] VARCHAR(5) NULL
)
