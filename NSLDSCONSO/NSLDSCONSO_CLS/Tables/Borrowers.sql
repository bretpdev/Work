CREATE TABLE [nsldsconso].[Borrowers]
(
	[BorrowerId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DataLoadRunId] INT NOT NULL, 
    [Ssn] CHAR(9) NOT NULL, 
	[Name] VARCHAR(100) NOT NULL,
	[DateOfBirth] DATE NOT NULL,
	[FileName] VARCHAR(50) NOT NULL,
	[ReportedToNsldsOn] DATETIME NULL,
	[ReportedToNsldsBy] VARCHAR(50) NULL,
	[AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
	[InactivatedOn] DATETIME NULL,
	[InactivatedBy] VARCHAR(50) NULL,
	CONSTRAINT [FK_Borrowers_BanaLoadRuns] FOREIGN KEY ([DataLoadRunId]) REFERENCES nsldsconso.DataLoadRuns([DataLoadRunId]), 
    CONSTRAINT [CK_Borrowers_Inactivated] CHECK (([InactivatedOn] IS NULL AND [InactivatedBy] IS NULL) OR ([InactivatedOn] IS NOT NULL AND [InactivatedBy] IS NOT NULL)),
	CONSTRAINT [CK_Borrowers_Reported] CHECK (([ReportedToNsldsOn] IS NULL AND [ReportedToNsldsBy] IS NULL) OR ([ReportedToNsldsOn] IS NOT NULL AND [ReportedToNsldsBy] IS NOT NULL))
)
