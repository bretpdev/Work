CREATE TABLE [dbo].[ManualBorrowerAlertMessages] (
    [ManualBorrowerAlertMessageId] INT            IDENTITY (1, 1) NOT NULL,
    [Message]                      VARCHAR (1000) NOT NULL,
    PRIMARY KEY CLUSTERED ([ManualBorrowerAlertMessageId] ASC)
);

