CREATE TABLE [dbo].[ProcessNotifications]
(
	[ProcessNotificationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NotificationTypeId] INT NOT NULL, 
    [NotificationSeverityTypeId] INT NOT NULL, 
    [ProcessLogId] INT NOT NULL, 
    CONSTRAINT [FK_ScriptNotifications_NotificationTypes] FOREIGN KEY ([NotificationTypeId]) REFERENCES [NotificationTypes]([NotificationTypeId]), 
    CONSTRAINT [FK_ScriptNotifications_NotificationSeverityTypes] FOREIGN KEY ([NotificationSeverityTypeId]) REFERENCES [NotificationSeverityTypes]([NotificationSeverityTypeId]), 
    CONSTRAINT [FK_ScriptNotifications_ScriptLogs] FOREIGN KEY ([ProcessLogId]) REFERENCES [ProcessLogs]([ProcessLogId])
)
