CREATE TABLE [dbo].[PaymentHistoryPaymentTypes] (
    [Type]        CHAR (4)     NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PaymentHistoryPaymentTypes] PRIMARY KEY CLUSTERED ([Type] ASC)
);

