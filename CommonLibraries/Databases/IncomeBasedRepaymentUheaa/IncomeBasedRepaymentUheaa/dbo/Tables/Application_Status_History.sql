CREATE TABLE [dbo].[Application_Status_History] (
    [ApplicationStatusHistoryId] INT          IDENTITY (1, 1) NOT NULL,
    [ApplicationId]              INT          NOT NULL,
    [UpdatedBy]                  VARCHAR (50) NOT NULL,
    [UpdatedAt]                  DATETIME     NOT NULL,
    [Active]                     BIT          NOT NULL,
    PRIMARY KEY CLUSTERED ([ApplicationStatusHistoryId] ASC)
);

