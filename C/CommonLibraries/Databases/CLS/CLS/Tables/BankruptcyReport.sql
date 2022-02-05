CREATE TABLE [dbo].[BankruptcyReport] (
    [FiscalYearToDateBorrowers] INT             NOT NULL,
    [FiscalYearToDateLoans]     INT             NOT NULL,
    [FiscalYearToDateAmount]    DECIMAL (22, 2) NOT NULL,
    [CornerStone]               BIT             NOT NULL
);

