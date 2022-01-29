CREATE TYPE [pridrcrp].[DisbursementRecord] AS TABLE
(
	[DisbursementDate] DATE NOT NULL,
	[InterestRate] DECIMAL(14,3) NOT NULL,
	[LoanType] VARCHAR(50) NOT NULL,
	[DisbursementNumber] VARCHAR(2) NOT NULL,
	[LoanId] VARCHAR(15) NOT NULL,
	[DisbursementAmount] DECIMAL(14,2) NOT NULL,
	[CapInterest] DECIMAL(14,2) NOT NULL,
	[RefundCancel] DECIMAL(14,2) NOT NULL,
	[BorrPaidPrin] DECIMAL(14,2) NOT NULL,
	[PrinOutstanding] DECIMAL(14,2) NOT NULL,
	[PaidInterest] DECIMAL(14,2) NOT NULL
)
