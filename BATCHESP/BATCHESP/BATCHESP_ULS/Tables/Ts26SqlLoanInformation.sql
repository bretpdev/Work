CREATE TABLE batchesp.Ts26LoanInformation
(
	[Ts26LoanInformationId]	INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[BorrowerSSN]			CHAR(9) NOT NULL,
	[LoanSequence]			SMALLINT NULL,
	[LoanProgramType]		VARCHAR(6) NULL,
	[CurrentPrincipal]		DECIMAL(8,2) NULL,
	[DisbursementDate]		DATE NULL,
	[GraceEndDate]			DATE NULL,
	[RepaymentStartDate]	DATE NULL,
	[EffectAddDate]			DATE NULL,
	[RehabRepurch]			VARCHAR(1) NULL,
	[TermBeg]				DATE NULL,
	[TermEnd]				DATE NULL,
	[CreatedAt]				DATETIME DEFAULT GETDATE() NOT NULL,
	[CreatedBy]				VARCHAR(50) DEFAULT SYSTEM_USER NOT NULL,
	[ProcessedAt]			DATETIME NULL,
	[ProcessedBy]			VARCHAR(50) NULL,
	[UpdatedAt]				DATETIME NULL
);
