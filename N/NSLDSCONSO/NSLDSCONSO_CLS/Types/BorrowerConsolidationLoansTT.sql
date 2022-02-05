CREATE TYPE [nsldsconso].[BorrowerConsolidationLoansTT] AS TABLE
(
	[NewLoanId] VARCHAR(50) NOT NULL,
	[GrossAmount] DECIMAL(14, 2) NOT NULL,
	[InterestRate] DECIMAL(7, 5) NOT NULL,
	[RebateAmount] DECIMAL(14, 2) NOT NULL,
	[NewGrossAmountSubsidized] DECIMAL(14, 2) NULL,
	[NewGrossAmountUnsubsidized] DECIMAL(14, 2) NULL,
	[NewInterestRate] DECIMAL(7, 5) NULL,
	[NewRebateSubsidized] DECIMAL(14, 2) NULL,
	[NewRebateUnsubsidized] DECIMAL(14, 2) NULL
)
