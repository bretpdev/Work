CREATE TABLE [dbo].[ExistingBorrowers] (
    [BF_SSN] [varchar](9) NULL,
	[CompassLoanSeq] [smallint] NULL,
	[MAX_DEF_CONTROL_NUM] [varchar](3) NULL,
	[MAX_FORB_CONTROL_NUM] [varchar](3) NULL
);

GO

CREATE CLUSTERED INDEX [CIX-CompassLoanSeq] ON [dbo].[ExistingBorrowers]
(
[BF_SSN] ASC,
[CompassLoanSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

GO