CREATE TABLE [dbo].[ArcLoanSequenceSelection] (
    [ArcLoanSequenceSelectionId] BIGINT NOT NULL IDENTITY,
    [ArcAddProcessingId]           BIGINT NOT NULL,
    [LoanSequence] INT NOT NULL,
    CONSTRAINT [FK_ArcLoanSequenceSelection_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [ArcAddProcessing]([ArcAddProcessingId]), 
    CONSTRAINT [PK_ArcLoanSequenceSelection] PRIMARY KEY ([ArcLoanSequenceSelectionId])
);

