CREATE TABLE [rcemailpul].[SendGridHistory]
(
	[SendGridHistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FromEmail] VARCHAR(320) NOT NULL,
    [MsgId] VARCHAR(50) NOT NULL, 
    [Subject] VARCHAR(200) NOT NULL, 
    [ToEmail] VARCHAR(320) NOT NULL, 
    [Status] VARCHAR(50) NOT NULL, 
    [OpensCount] INT NOT NULL, 
    [ClicksCount] INT NOT NULL, 
    [LastEventTime] DATETIME NOT NULL
)
