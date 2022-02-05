CREATE TABLE [dbo].[AY10_BR_LON_ATY](
	[BF_SSN] [char](9) NOT NULL,
	[LN_ATY_SEQ] [int] NOT NULL,
	[LD_ATY_RSP] [date] NULL,
	[LT_ATY_RSP] [time](0) NULL,
	[LF_ATY_RCP] [char](9) NULL,
	[LD_ATY_REQ_RCV] [date] NOT NULL,
	[LD_REQ_RSP_ATY_PRF] [date] NULL,
	[LF_USR_REQ_ATY] [varchar](8) NOT NULL,
	[LC_PRD_CAL] [varchar](6) NOT NULL,
	[LF_PRF_BY] [varchar](8) NOT NULL,
	[LC_STA_ACTY10] [char](1) NOT NULL,
	[LD_STA_ACTY10] [date] NOT NULL,
	[LI_ATY_MKP_GRC] [char](1) NOT NULL,
	[LC_ATY_RCP] [char](2) NOT NULL,
	[LF_LST_DTS_AY10] [datetime2](7) NOT NULL,
	[LC_ATY_RGD_TO] [char](1) NOT NULL,
	[LF_ATY_RGD_TO] [varchar](9) NULL,
	[PF_RSP_ACT] [varchar](5) NULL,
	[PF_REQ_ACT] [varchar](5) NOT NULL,
	[LF_COR_DOC] [varchar](17) NULL,
	[LD_COR_RCV_SNT] [date] NULL,
	[LC_COR_IN_OUT] [char](1) NULL,
	[AN_SEQ] [smallint] NULL,
	[IC_LON_PGM] [varchar](6) NULL,
	[AN_LC_APL_SEQ] [smallint] NULL,
 CONSTRAINT [PK_AY10_BR_LON_ATY] PRIMARY KEY CLUSTERED 
(
	[BF_SSN] ASC,
	[LN_ATY_SEQ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO