CREATE TABLE [batchesp].[ParentPlusLoanDetails]
(
	[ParentPlusLoanDetailsId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BorrowerSsn] CHAR(9) NOT NULL,
	[StudentSsn] CHAR(9) NOT NULL,
	[LoanSequence] INT NOT NULL,
	[DefermentRequested] BIT NOT NULL,
	[PostEnrollmentDefermentEligible] BIT NOT NULL,
	[CreatedAt] DATETIME DEFAULT GETDATE() NOT NULL,
	[CreatedBy]	VARCHAR(50) DEFAULT USER_NAME() NOT NULL,
	[ProcessedAt] DATETIME NULL,
	[ProcessedBy] VARCHAR(50) NULL,
	[UpdatedAt] DATETIME NULL
)
