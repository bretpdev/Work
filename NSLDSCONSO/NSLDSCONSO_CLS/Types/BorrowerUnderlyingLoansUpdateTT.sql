CREATE TYPE [nsldsconso].[BorrowerUnderlyingLoansUpdateTT] AS TABLE
(
	[BorrowerUnderlyingLoanId] INT NOT NULL,
	[NewLoanId] VARCHAR(50),
	[NsldsLabel] VARCHAR(50) NULL
)
