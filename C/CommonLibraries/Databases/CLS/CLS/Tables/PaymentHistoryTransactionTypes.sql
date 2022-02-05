CREATE TABLE [dbo].[PaymentHistoryTransactionTypes] (
    [Type]        CHAR (2)     NOT NULL,
    [Description] VARCHAR (50) NULL,
    CONSTRAINT [PK_PaymentHistoryTransactionTypes] PRIMARY KEY CLUSTERED ([Type] ASC)
);

