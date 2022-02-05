CREATE TABLE [projectrequest].[NotificationRecipients]
(
	[RecipientId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RecipientEmail] VARCHAR(200) NOT NULL,
	[Active] BIT NOT NULL
)
