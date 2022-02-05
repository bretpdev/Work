CREATE TABLE [dbo].[CompassPifLoanLevel] (
    [AccountNumber] VARCHAR (10)  NOT NULL,
    [UniqueId]      VARCHAR (50)  NOT NULL,
    [GuarDate]      VARCHAR (10)  NULL,
    [GuarAmt]       VARCHAR (20)  NOT NULL,
    [LoanPgm]       VARCHAR (100) NOT NULL,
    [LoanSeq]       INT           NOT NULL
);

