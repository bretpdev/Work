CREATE TABLE [schrpt].[SchoolEmailHistory]
(
	[SchoolEmailHistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SchoolRecipientId] INT NOT NULL, 
	[FileSent] VARCHAR(1000) NULL,
    [EmailSentAt] DATETIME NOT NULL, 
    [AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL,
	CONSTRAINT [FK_SchoolRecipients_SchoolRecipients] FOREIGN KEY ([SchoolRecipientId]) REFERENCES schrpt.SchoolRecipients([SchoolRecipientId])

)
