CREATE TABLE [alerts].[ManualBorrowerAlertMessages]
(
	[ManualBorrowerAlertMessageId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Message] VARCHAR(1000) NOT NULL,
	[StartDateInclusive] DATETIME NULL, 
    [EndDateInclusive] DATETIME NULL, 
    [AbortAfterMessageDisplay] BIT NOT NULL, 
)
