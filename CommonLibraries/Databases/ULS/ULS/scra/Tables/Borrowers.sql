CREATE TABLE [scra].[Borrowers] (
    [BorrowerId]            INT       IDENTITY (1, 1) NOT NULL,
    [BorrowerAccountNumber] CHAR (10) NULL,
    [EndorserAccountNumber] CHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([BorrowerId] ASC)
);

