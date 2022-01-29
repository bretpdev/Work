CREATE TABLE [dbo].[ArcLoanSequenceSelection] (
    [ArcLoanSequenceSelectionId] BIGINT IDENTITY (1, 1) NOT NULL,
    [ArcAddProcessingId]         BIGINT NOT NULL,
    [LoanSequence]               INT    NOT NULL,
    CONSTRAINT [PK_ArcLoanSequenceSelection] PRIMARY KEY CLUSTERED ([ArcLoanSequenceSelectionId] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_ArcLoanSequenceSelection_ArcAddProcessingId]
    ON [dbo].[ArcLoanSequenceSelection]([ArcAddProcessingId] ASC)
    INCLUDE([LoanSequence]) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);

