CREATE TABLE [dbo].[PD10_PRS_NME](
	[DF_PRS_ID] [char](9) NOT NULL,
	[DD_STA_PRS] [datetime] NULL,
	[DC_LAG_FGN] [char](1) NOT NULL,
	[DC_SEX] [char](1) NOT NULL,
	[DD_BRT] [datetime] NULL,
	[DM_PRS_MID] [varchar](13) NOT NULL,
	[DM_PRS_1] [varchar](13) NOT NULL,
	[DM_PRS_LST_SFX] [varchar](4) NOT NULL,
	[DM_PRS_LST] [varchar](23) NOT NULL,
	[DD_DRV_LIC_REN] [datetime] NULL,
	[DC_ST_DRV_LIC] [varchar](2) NOT NULL,
	[DF_DRV_LIC] [varchar](20) NOT NULL,
	[DD_NME_VER_LST] [datetime] NULL,
	[DI_ORG_HLD] [char](1) NOT NULL,
	[DF_LST_USR_PD10] [varchar](8) NOT NULL,
	[DF_ALN_RGS] [varchar](9) NOT NULL,
	[DI_US_CTZ] [char](1) NOT NULL,
	[DF_LST_DTS_PD10] [datetime] NOT NULL,
	[DF_SPE_ACC_ID] [varchar](10) NOT NULL,
	[DF_PRS_LST_4_SSN] [varchar](4) NOT NULL,
	[DI_ATU_FMT] [varchar](1) NOT NULL,
	[DC_ATU_FMT_TYP] [varchar](2) NOT NULL,
 CONSTRAINT [PK_PD10_PRS_NME_1] PRIMARY KEY CLUSTERED 
(
	[DF_PRS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]
