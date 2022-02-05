CREATE TABLE [log].[ProcessLogMessages]
(
	[ProcessLogMessageId] int not null identity(1,1) PRIMARY KEY,
	[ProcessNotificationId] int not null,
	[LogMessage] varchar(max) not null
)
