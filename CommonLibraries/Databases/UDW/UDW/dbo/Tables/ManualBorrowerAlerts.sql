CREATE TABLE [dbo].[ManualBorrowerAlerts] (
    [ManualBorrowerAlertId]        INT      IDENTITY (1, 1) NOT NULL,
    [Ssn]                          CHAR (9) NOT NULL,
    [ManualBorrowerAlertMessageId] INT      NOT NULL,
    [StartDateInclusive]           DATE     NOT NULL,
    [EndDateInclusive]             DATE     NOT NULL,
    PRIMARY KEY CLUSTERED ([ManualBorrowerAlertId] ASC),
    CONSTRAINT [FK_ManualBorrowerAlerts_ManualBorrowerAlertMessages] FOREIGN KEY ([ManualBorrowerAlertMessageId]) REFERENCES [dbo].[ManualBorrowerAlertMessages] ([ManualBorrowerAlertMessageId])
);

