CREATE TABLE [compfafsa].[NotificationRecipients]
(
	[NotificationRecipientId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Recipient] VARCHAR(200) NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(200) NULL
)
