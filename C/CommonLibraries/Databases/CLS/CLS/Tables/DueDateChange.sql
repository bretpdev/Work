CREATE TABLE [dbo].[DueDateChange]
(
	[DueDateChangeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Ssn] CHAR(9) NOT NULL,
	[AccountNumber] varchar(10) not null, 
    [DueDate] CHAR(2) NOT NULL, 
	[Arc] varchar(5) null,
	[Comment] varchar(300) null,
    [ProcessedAt] DATETIME NULL, 
	[Successful] bit null,
    [AddedAt] DATETIME NOT NULL DEFAULT GetDate()
)
