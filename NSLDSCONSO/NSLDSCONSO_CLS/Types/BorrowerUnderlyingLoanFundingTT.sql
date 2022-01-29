CREATE TYPE [nsldsconso].[BorrowerUnderlyingLoanFundingTT] AS TABLE
(
	[UnderlyingLoanId] VARCHAR(50) NOT NULL,
	[LoanType] CHAR(2) NOT NULL,
	[AmountType] CHAR(1) NOT NULL,
	[TotalAmount] DECIMAL(14, 2) NOT NULL,
	[RebateAmount] DECIMAL(14, 2) NULL,
	[DisbursementDate] DATE NOT NULL,
	[DisbursementNumber] INT NOT NULL,
	[DisbursementSequenceNumber] INT NOT NULL,
	[DateFunded] DATE NOT NULL,
	[LoanHolderId] VARCHAR(20) NOT NULL,
	[Comments] VARCHAR(50) NULL
)
