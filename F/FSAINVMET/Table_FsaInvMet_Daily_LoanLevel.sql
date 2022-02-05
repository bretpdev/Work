USE [CDW]
GO

/****** Object:  Table [FsaInvMet].[Daily_LoanLevel]    Script Date: 11/11/2019 4:19:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [FsaInvMet].[Daily_LoanLevel](
	[BF_SSN] [char](9) NOT NULL,
	[LN_SEQ] [smallint] NOT NULL,
	[LC_STA_LON10] [char](1) NOT NULL,
	[LD_STA_LON10] [date] NOT NULL,
	[LD_LON_EFF_ADD] [date] NULL,
	[LD_LON_ACL_ADD] [date] NULL,
	[LD_PIF_RPT] [date] NULL,
	[LC_CAM_LON_STA] [varchar](2) NOT NULL,
	[DX_ADR_EML] [varchar](254) NULL,
	[IC_LON_PGM] [varchar](2) NOT NULL,
	[LA_OTS_PRI_ELG] [numeric](8, 2) NULL,
	[WA_TOT_BRI_OTS] [numeric](12, 2) NULL,
	[LN_DLQ_MAX] [numeric](11, 0) NULL,
	[SPEC_FORB_IND] [varchar](1) NOT NULL,
	[WC_DW_LON_STA] [char](2) NULL,
	[ORD] [int] NOT NULL,
	[BILL_SATISFIED] [int] NOT NULL,
	[Segment] [int] NULL,
	[BorrSegment] [int] NULL,
	[LF_LON_CUR_OWN] [varchar](8) NOT NULL,
	[DefermentIndicator] [int] NOT NULL,
	[BorrDefermentIndicator] [int] NOT NULL,
	[PIF_TRN_DT] [date] NULL,
	[PerformanceCategory] [varchar](6) NULL,
	[ActiveMilitaryIndicator] [int] NULL,
	[LoanStatusPriority] [bigint] NULL,
	[LoanSegmentPriority] [bigint] NULL
) ON [PRIMARY]

GO

ALTER TABLE [FsaInvMet].[Daily_LoanLevel] ADD CONSTRAINT [PK_Daily_LoanLevel] PRIMARY KEY CLUSTERED
(
[BF_SSN] ASC
      ,[LN_SEQ] ASC
);

GO

