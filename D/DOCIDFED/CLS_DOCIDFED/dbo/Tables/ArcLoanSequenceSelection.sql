CREATE TABLE [dbo].[ArcLoanSequenceSelection] (
    [ArcLoanSequenceSelectionId] BIGINT IDENTITY (1, 1) NOT NULL,
    [ArcAddProcessingId]         BIGINT NOT NULL,
    [LoanSequence]               INT    NOT NULL,
    CONSTRAINT [PK_ArcLoanSequenceSelection] PRIMARY KEY NONCLUSTERED ([ArcLoanSequenceSelectionId] ASC),
    CONSTRAINT [FK_ArcLoanSequenceSelection_ArcAddProcessing1] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].[ArcAddProcessing] ([ArcAddProcessingId])
);




GO
CREATE CLUSTERED INDEX [IXC_ArcAddProcessingId]
    ON [dbo].[ArcLoanSequenceSelection]([ArcAddProcessingId] ASC, [LoanSequence] ASC);

