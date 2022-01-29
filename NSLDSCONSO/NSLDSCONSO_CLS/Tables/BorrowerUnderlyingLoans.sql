CREATE TABLE [nsldsconso].[BorrowerUnderlyingLoans]
(
	[BorrowerUnderlyingLoanId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BorrowerId] INT NOT NULL,
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
	[UnderlyingLoanBalance] DECIMAL(14, 2) NOT NULL,
	[AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    CONSTRAINT [FK_BorrowerUnderlyingLoans_Borrowers] FOREIGN KEY ([BorrowerId]) REFERENCES [nsldsconso].[Borrowers]([BorrowerId])
)
