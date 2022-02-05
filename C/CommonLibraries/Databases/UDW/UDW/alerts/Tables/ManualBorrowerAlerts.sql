CREATE TABLE [alerts].[ManualBorrowerAlerts] (
    [ManualBorrowerAlertId]        INT      IDENTITY (1, 1) NOT NULL,
    [Ssn]                          CHAR (9) NOT NULL,
    [ManualBorrowerAlertMessageId] INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([ManualBorrowerAlertId] ASC),
    CONSTRAINT [FK_ManualBorrowerAlerts_ManualBorrowerAlertMessages] FOREIGN KEY ([ManualBorrowerAlertMessageId]) REFERENCES [alerts].[ManualBorrowerAlertMessages] ([ManualBorrowerAlertMessageId])
);

