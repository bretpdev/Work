CREATE TABLE [alerts].[ManualBorrowerAlerts]
(
	[ManualBorrowerAlertId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Ssn] CHAR(9) NOT NULL, 
    [ManualBorrowerAlertMessageId] INT NOT NULL
    CONSTRAINT [FK_ManualBorrowerAlerts_ManualBorrowerAlertMessages] FOREIGN KEY ([ManualBorrowerAlertMessageId]) REFERENCES [alerts].[ManualBorrowerAlertMessages]([ManualBorrowerAlertMessageId])
)
