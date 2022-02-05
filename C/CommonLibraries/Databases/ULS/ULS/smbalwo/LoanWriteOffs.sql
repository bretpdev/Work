CREATE TABLE [smbalwo].[LoanWriteOffs]
(
	[LoanWriteOffId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DF_SPE_ACC_ID] VARCHAR(10) NOT NULL, 
    [LN_SEQ] INT NOT NULL, 
    [LoanBalance] DECIMAL(18, 2) NOT NULL, 
	[IsDelinquent] BIT NOT NULL,
	[ActualPrincipalWrittenOff] DECIMAL(18,2) NULL,
	[ActualInterestWrittenOff] DECIMAL(18,2) NULL,
    [ProcessedAt] DATETIME NULL, 
	[HadError] BIT NOT NULL DEFAULT(0),
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(100) NULL
)
