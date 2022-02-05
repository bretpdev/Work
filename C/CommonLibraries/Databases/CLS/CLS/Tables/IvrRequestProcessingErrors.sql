CREATE TABLE [dbo].[IvrRequestProcessingErrors] (
    [RecNum]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [AccountNumber] VARCHAR (10) NOT NULL,
    [Request]       VARCHAR (60) NOT NULL
);

