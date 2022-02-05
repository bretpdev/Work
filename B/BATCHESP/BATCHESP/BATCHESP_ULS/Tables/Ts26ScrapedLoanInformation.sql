CREATE TABLE [batchesp].[Ts26ScrapedLoanInformation]
(
	[Ts26ScrapedLoanInformationId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BorrowerSsn]			CHAR(9) NOT NULL,
	[LoanSequence]			SMALLINT NOT NULL,
	[LoanStatus]			VARCHAR(40) NOT NULL,
	[RepaymentStartDate]	DATE NULL,
	[CreatedAt]				DATETIME DEFAULT GETDATE() NOT NULL,
	[CreatedBy]				VARCHAR(50) DEFAULT SUSER_SNAME() NOT NULL,
	[ProcessedAt]			DATETIME NULL,
	[ProcessedBy]			VARCHAR(50) NULL,
	[UpdatedAt]				DATETIME NULL
)
