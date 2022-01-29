CREATE TABLE [clschllnfd].[ErrorLogs]
(
	[ErrorLogId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BorrowerSsn] CHAR(9) NOT NULL, 
    [AccountNumber] CHAR(10) NOT NULL, 
    [LoanSeq] INT NOT NULL, 
    [DisbursementSeq] INT NOT NULL, 
    [Arc] VARCHAR(5) NOT NULL, 
    [ErrorMessage] VARCHAR(300) NOT NULL, 
    [SessionMessage] VARCHAR(300) NOT NULL, 
    [SchoolClosureDataId] INT NOT NULL, 
    [ArcAddProcessingId] BIGINT NULL,
	[AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(25) NOT NULL DEFAULT SUSER_SNAME(), 
	CONSTRAINT [FK_ErrorLogs_SchoolClosureData] FOREIGN KEY ([SchoolClosureDataId]) REFERENCES [clschllnfd].SchoolClosureData ([SchoolClosureDataId]),
	CONSTRAINT [FK_ErrorLogs_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].ArcAddProcessing ([ArcAddProcessingId])
)
