CREATE TABLE [dbo].[LN90_FIN_ATY](
	[BF_SSN] [varchar](9) NOT NULL,
	[LN_SEQ] [smallint] NOT NULL,
	[LN_FAT_SEQ] [smallint] NOT NULL,
	[LC_FAT_REV_REA] [char](1) NULL,
	[LD_FAT_APL] [datetime] NULL,
	[LD_FAT_PST] [datetime] NULL,
	[LD_FAT_EFF] [datetime] NULL,
	[LD_FAT_DPS] [datetime] NULL,
	[LC_CSH_ADV] [char](1) NULL,
	[LD_STA_LON90] [datetime] NULL,
	[LC_STA_LON90] [char](1) NULL,
	[LA_FAT_PCL_FEE] [decimal](7, 2) NULL,
	[LA_FAT_NSI] [decimal](7, 2) NULL,
	[LA_FAT_LTE_FEE] [decimal](7, 2) NULL,
	[LA_FAT_ILG_PRI] [decimal](8, 2) NULL,
	[LA_FAT_CUR_PRI] [decimal](8, 2) NULL,
	[LF_LST_DTS_LN90] [datetime] NULL,
	[PC_FAT_TYP] [varchar](2) NULL,
	[PC_FAT_SUB_TYP] [varchar](2) NULL,
	[LA_FAT_NSI_ACR] [decimal](7, 2) NULL,
	[LI_FAT_RAP] [char](1) NULL,
	[LN_FAT_SEQ_REV] [smallint] NULL,
	[LI_EFT_NSF_OVR] [char](1) NULL,
	[LF_USR_EFT_NSF_OVR] [varchar](8) NULL,
	[LA_FAT_MSC_FEE] [decimal](12, 2) NULL,
	[LA_FAT_MSC_FEE_PCV] [decimal](12, 2) NULL,
	[LA_FAT_DL_REB] [decimal](12, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[BF_SSN] ASC,
	[LN_SEQ] ASC,
	[LN_FAT_SEQ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO