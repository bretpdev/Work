CREATE TABLE [dbo].[DW01_DW_CLC_CLU](
	[BF_SSN] [char](9) NOT NULL,
	[LN_SEQ] [smallint] NOT NULL,
	[WD_CLC_THU] [date] NOT NULL,
	[LA_NSI_OTS] [numeric](12, 2) NULL,
	[LA_NSI_ACR] [numeric](12, 2) NULL,
	[WA_TOT_BRI_OTS] [numeric](12, 2) NULL,
	[WC_DW_LON_STA] [char](2) NOT NULL,
	[WD_LON_RPD_SR] [date] NULL,
	[WD_XPC_POF_TS26] [date] NULL,
	[WX_OVR_DW_LON_STA] [char](20) NULL,
	[WA_STD_STD_ISL] [numeric](12, 2) NULL,
	[WC_LON_STA_GRC] [varchar](2) NOT NULL,
	[WC_LON_STA_SCL] [varchar](2) NOT NULL,
	[WC_LON_STA_RPY] [varchar](2) NOT NULL,
	[WC_LON_STA_DFR] [varchar](2) NOT NULL,
	[WC_LON_STA_FOR] [varchar](2) NOT NULL,
	[WC_LON_STA_CUR] [varchar](2) NOT NULL,
	[WC_LON_STA_CLM] [varchar](2) NOT NULL,
	[WC_LON_STA_PCL] [varchar](2) NOT NULL,
	[WC_LON_STA_DTH] [varchar](2) NOT NULL,
	[WC_LON_STA_DSA] [varchar](2) NOT NULL,
	[WC_LON_STA_BKR] [varchar](2) NOT NULL,
	[WC_LON_STA_PIF] [varchar](2) NOT NULL,
	[WC_LON_STA_FUL_ORG] [varchar](2) NOT NULL,
	[WC_LON_DFR_FOR_TYP] [varchar](2) NOT NULL,
 CONSTRAINT [PK_DW01_DW_CLC_CLU] PRIMARY KEY CLUSTERED 
(
	[BF_SSN] ASC,
	[LN_SEQ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO