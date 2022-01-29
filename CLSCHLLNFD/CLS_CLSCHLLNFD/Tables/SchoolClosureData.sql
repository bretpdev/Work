CREATE TABLE [clschllnfd].[SchoolClosureData]
(
	[SchoolClosureDataId] INT NOT NULL PRIMARY KEY IDENTITY,
    [BorrowerSsn] CHAR(9) NOT NULL, 
    [StudentSsn] CHAR(9) NULL, 
    [LoanSeq] INT NOT NULL, 
    [DisbursementSeq] INT NOT NULL, 
    [DischargeAmount] MONEY NOT NULL,
	[DischargeDate] DATETIME NOT NULL, 
    [SchoolCode] CHAR(8) NOT NULL, 
    [ArcAddProcessingId] BIGINT NULL, 
	[PrintProcessingId] INT NULL,
    [ProcessedAt] DATETIME NULL, 
	[FinalArcAddProcessingId] bigint,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(25) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(25) NULL, 
	[WasProcessedPrior] BIT NULL
    CONSTRAINT [FK_SchoolClosureData_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [ArcAddProcessing]([ArcAddProcessingId])
)
