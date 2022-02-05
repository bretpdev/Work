CREATE TABLE [batchesp].[TsayScrapedLoanInformation]
(
	[TsayScrapedLoanInformationId] INT IDENTITY(1,1) NOT NULL, 
    [BorrowerSsn] CHAR(9) NOT NULL, 
    [LoanSequence] INT NULL, 
    [LoanProgramType] VARCHAR(6) NULL, 
    [Type] VARCHAR(9) NULL, 
    [DeferSchool] VARCHAR(8) NULL, 
    [BeginDate] DATE NULL, 
    [EndDate] DATE NULL, 
    [CertificationDate] DATE NULL, 
    [AppliedDate] DATE NULL, 
    [DisbursementDate] DATE NULL, 
    [ApprovalStatus] VARCHAR(15) NULL, 
	[SourceModule] VARCHAR(100) NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [CreatedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
	[ProcessedAt] DATETIME NULL,
	[ProcessedBy] VARCHAR(50) NULL
)
