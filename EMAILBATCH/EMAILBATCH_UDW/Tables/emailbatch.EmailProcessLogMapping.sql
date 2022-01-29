CREATE TABLE [emailbatch].[EmailProcessLogMapping]
(
	[EmailProcessLogMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
	EmailProcessingId INT NOT NULL,
    [ProcessNotificationId] INT NOT NULL
)
