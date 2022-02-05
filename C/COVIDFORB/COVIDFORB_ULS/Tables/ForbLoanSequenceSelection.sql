CREATE TABLE [covidforb].ForbLoanSequenceSelection(
	ForbLoanSequenceSelectionId [BIGINT] IDENTITY(1,1) NOT NULL,
	ForbearanceProcessingId [BIGINT] NOT NULL,
	LoanSequence [SMALLINT] NOT NULL
	PRIMARY KEY NONCLUSTERED 
	(
		[ForbLoanSequenceSelectionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [covidforb].[ForbLoanSequenceSelection]  WITH NOCHECK ADD FOREIGN KEY([ForbearanceProcessingId])
REFERENCES [covidforb].[ForbearanceProcessing] ([ForbearanceProcessingId])
GO