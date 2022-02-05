CREATE TABLE [nsldsconso].[BorrowerConsolidationLoans]
(
	[BorrowerLoanInformationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BorrowerId] INT NOT NULL, 
	[NewLoanId] VARCHAR(50) NOT NULL,
	[GrossAmount] DECIMAL(14, 2) NOT NULL,
	[InterestRate] DECIMAL(7, 5) NOT NULL,
	[RebateAmount] DECIMAL(14, 2) NOT NULL,
	[NewGrossAmountSubsidized] DECIMAL(14, 2) NULL,
	[NewGrossAmountUnsubsidized] DECIMAL(14, 2) NULL,
	[NewInterestRate] DECIMAL(7, 5) NULL,
	[NewRebateSubsidized] DECIMAL(14, 2) NULL,
	[NewRebateUnsubsidized] DECIMAL(14, 2) NULL,
	[AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    CONSTRAINT [FK_BorrowerLoanInformation_Borrowers] FOREIGN KEY ([BorrowerId]) REFERENCES [nsldsconso].[Borrowers]([BorrowerId])
)
