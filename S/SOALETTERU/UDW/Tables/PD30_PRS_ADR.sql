CREATE TABLE [dbo].[PD30_PRS_ADR](
	[DF_PRS_ID] [varchar](9) NOT NULL,
	[DC_ADR] [char](1) NOT NULL,
	[DD_STA_PDEM30] [date] NULL,
	[DD_VER_ADR] [date] NULL,
	[DI_VLD_ADR] [char](1) NOT NULL,
	[DF_ZIP_CDE] [varchar](17) NOT NULL,
	[DM_CT] [varchar](20) NOT NULL,
	[DX_STR_ADR_3] [varchar](30) NOT NULL,
	[DX_STR_ADR_2] [varchar](30) NOT NULL,
	[DX_STR_ADR_1] [varchar](30) NOT NULL,
	[DC_DOM_ST] [varchar](2) NOT NULL,
	[DF_LST_USR_PD30] [varchar](8) NOT NULL,
	[DF_3PT_ADR] [varchar](9) NOT NULL,
	[DC_3PT_ADR] [varchar](2) NOT NULL,
	[DC_SRC_ADR] [varchar](2) NOT NULL,
	[DM_FGN_CNY] [varchar](25) NOT NULL,
	[DM_FGN_ST] [varchar](15) NOT NULL,
	[DF_LST_DTS_PD30] [datetime2](7) NULL,
	[DX_DLV_PTR_BCD] [varchar](14) NULL,
	[DC_FGN_CNY] [varchar](2) NOT NULL,
	[DD_DSB_ADR_BEG] [date] NULL,
	[DD_DSB_ADR_END] [date] NULL,
 CONSTRAINT [PK_PD30_PRS_ADR_1] PRIMARY KEY CLUSTERED 
(
	[DF_PRS_ID] ASC,
	[DC_ADR] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO
