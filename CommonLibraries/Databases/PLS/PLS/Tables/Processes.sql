CREATE TABLE [ls008].[Processes]
(
	[ProcessId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProcessName] VARCHAR(300) NOT NULL, 
	[Description] VARCHAR(300) NOT NULL,
    [ARC] VARCHAR(5) NOT NULL, 
	[ArcMessageText] VARCHAR(1000) NULL,
	[OriginalCommentText] VARCHAR(300) NOT NULL,
    [DLLName] VARCHAR(20) NOT NULL, 
    [ClassName] VARCHAR(100) NOT NULL
)
