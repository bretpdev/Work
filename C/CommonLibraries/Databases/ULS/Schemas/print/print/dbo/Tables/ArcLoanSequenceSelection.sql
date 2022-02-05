CREATE TABLE [dbo].[ArcLoanSequenceSelection] (
    [ArcLoanSequenceSelectionId] BIGINT IDENTITY (1, 1) NOT NULL,
    [ArcAddProcessingId]         BIGINT NOT NULL,
    [LoanSequence]               INT    NOT NULL,
    CONSTRAINT [PK_ArcLoanSequenceSelection] PRIMARY KEY CLUSTERED ([ArcLoanSequenceSelectionId] ASC),
    CONSTRAINT [FK_ArcLoanSequenceSelection_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].[ArcAddProcessing] ([ArcAddProcessingId])
);

