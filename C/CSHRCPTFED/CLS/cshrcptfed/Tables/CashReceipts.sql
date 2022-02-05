CREATE TABLE [cshrcptfed].[CashReceipts] (
    [CashReceiptId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [Account]       VARCHAR (10)    NULL,
    [AmountRecvd]   DECIMAL (18, 2) NULL,
    [CheckNum]      VARCHAR (15)    NULL,
    [DateRecvd]     DATETIME        NULL,
    [Payee]         INT             NOT NULL,
    [AddedBy]       VARCHAR (32)    NULL,
    [AddedAt]       DATETIME        NULL,
    [ProcessedOn]   DATE            NULL,
    [Borrower]      VARCHAR (48)    NULL,
    [ArcId]         BIGINT          NULL,
    PRIMARY KEY CLUSTERED ([CashReceiptId] ASC) WITH (FILLFACTOR = 95)
);







