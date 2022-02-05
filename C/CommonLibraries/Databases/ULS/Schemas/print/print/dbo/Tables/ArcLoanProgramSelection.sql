CREATE TABLE [dbo].[ArcLoanProgramSelection] (
    [ArcLoanProgramSelectionId] BIGINT   IDENTITY (1, 1) NOT NULL,
    [ArcAddProcessingId]        BIGINT   NOT NULL,
    [LoanProgram]               CHAR (6) NOT NULL,
    PRIMARY KEY CLUSTERED ([ArcLoanProgramSelectionId] ASC),
    CONSTRAINT [FK_ArcLoanProgramSelection_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].[ArcAddProcessing] ([ArcAddProcessingId])
);

