CREATE TYPE [nsldsconso].[BorrowerUnderlyingLoansTT] AS TABLE
(
	[NewLoanId] VARCHAR(50) NULL,
	[UnderlyingLoanId] VARCHAR(50) NOT NULL,
	[NsldsLabel] VARCHAR(50) NULL,
	[LoanType] CHAR(2) NOT NULL,
	[FirstDisbursement] DATETIME NOT NULL,
	[LoanStatus] CHAR(2) NOT NULL,
	[JointConsolidationIndicator] CHAR(1) NOT NULL,
	[ParentPlusLoanFlag] CHAR(1) NOT NULL,
	[ForcedIdrFlag] CHAR(1) NOT NULL,
	[IdrStartDate] DATE NULL,
	[EconomicHardshipDefermentDaysUsed] INT,
	[LossOfSubsidyStatus] CHAR(1),
	[LossOfSubsidyStatusDate] DATE NULL,
	[InterestRate] DECIMAL(14, 5) NOT NULL,
	[UnderlyingLoanBalance] DECIMAL(14, 2) NOT NULL
)
