CREATE TYPE [pridrcrp].[MonetaryHistoryRecord] AS TABLE
(
	[Ssn] VARCHAR(9) NOT NULL,
	[LoanNum] INT NOT NULL,
	[TransactionDate] DATE NOT NULL,
	[PostDate] DATE NOT NULL,
	[TransactionCode] VARCHAR(5) NOT NULL,
	[CancelCode] VARCHAR(5) NULL,
	[TransactionAmount] DECIMAL(14,2) NOT NULL,
	[AppliedPrincipal] DECIMAL(14,2) NOT NULL,
	[AppliedInterest] DECIMAL(14,2) NOT NULL,
	[AppliedFees] DECIMAL(14,2) NOT NULL,
	[PrincipalBalanceAfterTransaction] DECIMAL(14,2) NOT NULL,
	[InterestBalanceAfterTransaction] DECIMAL(14,5) NOT NULL,
	[FeesBalanceAfterTransaction] DECIMAL(14,2) NOT NULL,
	[LoanSequence] INT NULL
)
