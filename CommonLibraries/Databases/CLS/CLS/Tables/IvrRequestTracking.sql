CREATE TABLE [dbo].[IvrRequestTracking] (
    [RecNum]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [AccountNumber] VARCHAR (10) NOT NULL,
    [Request]       VARCHAR (10) NOT NULL,
    [ProcessedDate] DATETIME     NULL,
    [PaymentAmount] VARCHAR (10) NULL,
    [CbpConfNum]    INT          NULL
);

