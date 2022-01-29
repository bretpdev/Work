--SASR_4616 PROMOTION FILE

USE [ODW]
GO

/****** Object:  Table [dbo].[DC11_LON_FAT]    Script Date: 12/6/2019 3:37:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DC11_LON_FAT](
	[AF_APL_ID] [varchar](17) NOT NULL,
	[AF_APL_ID_SFX] [varchar](2) NOT NULL,
	[LF_CRT_DTS_DC10] [datetime2](7) NOT NULL,
	[LF_CRT_DTS_DC11] [datetime2](7) NOT NULL,
	[BF_SSN] [varchar](9) NOT NULL,
	[BD_TRX_PST_HST] [date] NOT NULL,
	[BN_DLY_RCI_SEQ_HST] [smallint] NOT NULL,
	[LC_TRX_SRC] [varchar](2) NOT NULL,
	[LC_TRX_TYP] [varchar](2) NOT NULL,
	[LC_RCI_TYP] [varchar](2) NOT NULL,
	[LC_REV_IND_TYP] [varchar](2) NOT NULL,
	[LA_TRX] [numeric](9, 2) NOT NULL,
	[LD_TRX_EFF] [date] NULL,
	[LD_TRX_ADJ] [date] NULL,
	[LA_PRI_AT_PST] [numeric](9, 2) NOT NULL,
	[LA_INT_ACR_THS_PRD] [numeric](9, 2) NOT NULL,
	[LA_OTH_CHR_ACR] [numeric](7, 2) NOT NULL,
	[LA_TXO_FEE] [numeric](7, 2) NOT NULL,
	[LA_INJ_SPO_CLM] [numeric](9, 2) NOT NULL,
	[LA_APL_PRI] [numeric](9, 2) NOT NULL,
	[LA_APL_INT] [numeric](9, 2) NOT NULL,
	[LA_APL_LEG_CST] [numeric](7, 2) NOT NULL,
	[LA_APL_OTH_CHR] [numeric](7, 2) NOT NULL,
	[LA_APL_COL_CST] [numeric](7, 2) NOT NULL,
	[LA_OV_PAY] [numeric](9, 2) NOT NULL,
	[LD_OV_PAY_RFD] [date] NULL,
	[LC_RFD_IVC_TRG] [varchar](2) NOT NULL,
	[LC_DIR_PAY] [varchar](2) NOT NULL,
	[LF_CLR] [varchar](8) NOT NULL,
	[LF_OSE_CLR] [varchar](8) NOT NULL,
	[LF_USR_PST_TXN] [varchar](8) NOT NULL,
	[LA_COL_CST] [numeric](7, 2) NOT NULL,
	[LA_CLR_CMS] [numeric](7, 2) NOT NULL,
	[LD_RCC_TO_CLR] [date] NULL,
	[LD_CLA_FEE_REV] [date] NULL,
	[LF_SCN_SEQ] [varchar](9) NOT NULL,
	[LD_BCH] [date] NULL,
	[LN_BCH_SEQ] [smallint] NULL,
	[LA_PRI_RPT_DOE] [numeric](9, 2) NOT NULL,
	[LA_INT_RPT_DOE] [numeric](9, 2) NOT NULL,
	[LA_OTH_RPT_DOE] [numeric](9, 2) NOT NULL,
	[LA_CPE_RPT_DOE] [numeric](9, 2) NOT NULL,
	[LD_RCO] [date] NULL,
	[LD_RPT_DOE] [date] NULL,
	[LD_DUE_RCO_REV] [date] NULL,
	[LC_RCO] [varchar](2) NOT NULL,
	[LF_CON_RPR_LTR] [varchar](8) NOT NULL,
	[LF_LST_DTS_DC11] [datetime2](7) NOT NULL,
	[LR_COL_CST_PER] [numeric](6, 3) NOT NULL,
	[LR_CMS_PER] [numeric](6, 3) NOT NULL,
	[LF_USR_PST_REV] [varchar](8) NOT NULL,
	[LD_RMT_SUS] [date] NULL,
	[LC_ADV_TYP] [char](1) NOT NULL,
	[LD_BR_RFD_ETR_FD01] [date] NULL,
	[LN_BR_RFD_VT] [varchar](8) NOT NULL,
	[LI_RCV_CHK_OLY] [char](1) NOT NULL,
	[LA_CPE_RPT_DOE_ACL] [numeric](9, 2) NOT NULL,
	[LC_ADV_DCH_SRC] [char](1) NOT NULL,
	[LA_RHB_PAY_FUD_LDR] [numeric](12, 2) NOT NULL,
	[LA_RHB_PAY_FUD_GTR] [numeric](12, 2) NOT NULL,
	[LC_PAY_ISR] [char](1) NOT NULL,
	[LD_TRX_PST_ORG] [date] NULL,
	[LN_DLY_RCI_SEQ_ORG] [smallint] NULL,
	[LF_CRT_DTS_DC12] [datetime2](7) NULL,
	[LI_RHB_PAY_SLD_DOE] [char](1) NOT NULL,
	[LC_REV_IND_SUB_TYP] [varchar](3) NOT NULL,
 CONSTRAINT [PK_DC11_LON_FAT] PRIMARY KEY CLUSTERED 
(
	[AF_APL_ID] ASC,
	[AF_APL_ID_SFX] ASC,
	[LF_CRT_DTS_DC10] ASC,
	[LF_CRT_DTS_DC11] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO


