USE [UDW]
GO

/****** Object:  Table [uhedaymet].[Daily_LoanLevel]    Script Date: 11/12/2019 8:08:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [uhedaymet].[Daily_LoanLevel](
	[BF_SSN] [char](9) NOT NULL,
	[LN_SEQ] [smallint] NOT NULL,
	[LC_STA_LON10] [char](1) NULL,
	[LD_STA_LON10] [date] NULL,
	[LD_LON_EFF_ADD] [date] NULL,
	[LD_LON_ACL_ADD] [date] NULL,
	[LD_PIF_RPT] [date] NULL,
	[LC_CAM_LON_STA] [varchar](2) NULL,
	[DX_ADR_EML] [varchar](254) NULL,
	[IC_LON_PGM] [varchar](2) NULL,
	[LA_OTS_PRI_ELG] [numeric](12, 2) NULL,
	[WA_TOT_BRI_OTS] [numeric](12, 2) NULL,
	[LN_DLQ_MAX] [numeric](11, 0) NULL,
	[SPEC_FORB_IND] [varchar](1) NULL,
	[WC_DW_LON_STA] [char](2) NULL,
	[ORD] [int] NULL,
	[BILL_SATISFIED] [int] NULL,
	[Segment] [int] NULL,
	[BorrSegment] [int] NULL,
	[LF_LON_CUR_OWN] [varchar](8) NULL,
	[DefermentIndicator] [int] NULL,
	[BorrDefermentIndicator] [int] NULL,
	[PIF_TRN_DT] [date] NULL,
	[PerformanceCategory] [varchar](6) NULL,
	[ActiveMilitaryIndicator] [int] NULL,
	[LoanStatusPriority] [bigint] NULL,
	[LoanSegmentPriority] [bigint] NULL
)
GO


