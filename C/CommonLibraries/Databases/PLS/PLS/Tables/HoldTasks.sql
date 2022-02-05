CREATE TABLE [ls008].[HoldTasks]
(
	[HoldTaskId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[TaskControlNumber] VARCHAR(25) NOT NULL,
    [DocumentControlNumber] VARCHAR(18) NOT NULL, 
    [AccountNumber] CHAR(10) NOT NULL,  
    [ActivitySeq] VARCHAR(5) NOT NULL, 
    [FollowUpDate] DATETIME NOT NULL, 
	[PheaaUserId] VARCHAR(8) NOT NULL,
	[HoldReason] varchar(300),
    [AddedAt] DATETIME NOT NULL DEFAULT getdate()
)
