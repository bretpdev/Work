CREATE TABLE [dbo].[ArcLoanProgramSelection]
(
	[ArcLoanProgramSelectionId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ArcAddProcessingId] BIGINT NOT NULL, 
    [LoanProgram] CHAR(6) NOT NULL, 
    CONSTRAINT [FK_ArcLoanProgramSelection_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [ArcAddProcessing]([ArcAddProcessingId])
)
