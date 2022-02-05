USE [CLS] /* Change to use the CLS database to create the Notification table and insert procedure */
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProcessLogMessages')
DROP TABLE [log].ProcessLogMessages
GO
CREATE TABLE [log].ProcessLogMessages
(
	[ProcessLogMessageId] int not null identity(1,1) PRIMARY KEY,
	[ProcessNotificationId] int not null,
	[LogMessage] varchar(max) not null
)
GO