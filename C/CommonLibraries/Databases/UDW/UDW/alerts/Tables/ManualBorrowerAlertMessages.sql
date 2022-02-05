CREATE TABLE [alerts].[ManualBorrowerAlertMessages] (
    [ManualBorrowerAlertMessageId] INT            IDENTITY (1, 1) NOT NULL,
    [Message]                      VARCHAR (1000) NOT NULL,
    [StartDateInclusive]           DATETIME       NULL,
    [EndDateInclusive]             DATETIME       NULL,
    [AbortAfterMessageDisplay]     BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([ManualBorrowerAlertMessageId] ASC)
);

