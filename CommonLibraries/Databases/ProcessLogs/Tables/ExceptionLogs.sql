CREATE TABLE [dbo].[ExceptionLogs]
(
	[ExceptionLogId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ProcessLogId] INT NOT NULL,
    [ProcessNotificationId] INT NULL, 
    [CreatedOn] DATETIME NOT NULL DEFAULT getdate(), 
    [ExceptionType] VARCHAR(MAX) NOT NULL, 
    [AssemblyLocation] VARCHAR(MAX) NOT NULL, 
    [AssemblyFullName] VARCHAR(MAX) NOT NULL, 
	[AssemblyLastModified] datetime not null,
	[ExceptionSource] VARCHAR(MAX) NOT NULL, 
    [ExceptionMessage] VARCHAR(MAX) NOT NULL, 
    [StackTrace] VARCHAR(MAX) NOT NULL, 
    [FullDetails] VARCHAR(MAX) NOT NULL, 
    [LoggedBy] NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    [ResolvedBy] NVARCHAR(50) NULL, 
    [ResolutionDate] DATETIME NULL, 
    CONSTRAINT [FK_ExceptionLogs_ToProcessLogs] FOREIGN KEY (ProcessLogId) REFERENCES [ProcessLogs]([ProcessLogId]), 
    CONSTRAINT [FK_ExceptionLogs_ProcessNotifications] FOREIGN KEY ([ProcessNotificationId]) REFERENCES [ProcessNotifications]([ProcessNotificationId]) 
)

GO