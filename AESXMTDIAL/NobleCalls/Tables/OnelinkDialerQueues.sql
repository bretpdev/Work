CREATE TABLE [aesxmtdial].[OnelinkDialerQueues]
(
	[OnelinkDialerQueuesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Region] VARCHAR(3) NOT NULL, 
    [Type] CHAR NOT NULL, 
    [Queue] VARCHAR(8) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Alias] VARCHAR(50) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT USER_NAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARBINARY(50) NULL
)
