CREATE TABLE [pridrcrp].[MonetaryHistory] (
    [MonetaryHistoryId]                INT             IDENTITY (1, 1) NOT NULL,
    [Ssn]                              VARCHAR (9)     NOT NULL,
    [LoanNum]                          INT             NOT NULL,
    [TransactionDate]                  DATE            NOT NULL,
    [PostDate]                         DATE            NOT NULL,
    [TransactionCode]                  VARCHAR (5)     NOT NULL,
    [CancelCode]                       VARCHAR (5)     NULL,
    [TransactionAmount]                DECIMAL (14, 2) NOT NULL,
    [AppliedPrincipal]                 DECIMAL (14, 2) NOT NULL,
    [AppliedInterest]                  DECIMAL (14, 2) NOT NULL,
    [AppliedFees]                      DECIMAL (14, 2) NOT NULL,
    [PrincipalBalanceAfterTransaction] DECIMAL (14, 2) NOT NULL,
    [InterestBalanceAfterTransaction]  DECIMAL (14, 5) NOT NULL,
    [FeesBalanceAfterTransaction]      DECIMAL (14, 2) NOT NULL,
    [LoanSequence]                     INT             NULL,
	[PaymentSatisfactionProcessed] BIT DEFAULT(0) NOT NULL,
    PRIMARY KEY CLUSTERED ([MonetaryHistoryId] ASC) WITH (FILLFACTOR = 95)
);
GO

CREATE NONCLUSTERED INDEX [IX_MonetaryHistory_SSN]
    ON [pridrcrp].[MonetaryHistory]([Ssn] ASC, [LoanNum] ASC, [TransactionDate] ASC, [PostDate] ASC, [TransactionCode] ASC, [CancelCode] ASC, [TransactionAmount] ASC, [AppliedPrincipal] ASC, [AppliedInterest] ASC, [AppliedFees] ASC, [PrincipalBalanceAfterTransaction] ASC, [InterestBalanceAfterTransaction] ASC, [FeesBalanceAfterTransaction] ASC) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);
GO

