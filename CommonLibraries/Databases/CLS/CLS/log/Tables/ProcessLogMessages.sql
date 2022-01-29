CREATE TABLE [log].[ProcessLogMessages] (
    [ProcessLogMessageId]   INT           IDENTITY (1, 1) NOT NULL,
    [ProcessNotificationId] INT           NOT NULL,
    [LogMessage]            VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProcessLogMessageId] ASC)
);

