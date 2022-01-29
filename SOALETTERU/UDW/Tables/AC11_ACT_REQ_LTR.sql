CREATE TABLE [dbo].[AC11_ACT_REQ_LTR](
	[PF_REQ_ACT] [varchar](5) NOT NULL,
	[PF_LTR] [varchar](10) NOT NULL,
	[PF_LST_DTS_AC11] [datetime2](7) NOT NULL,
	[PI_BKR_EDS_SND_LTR] [char](1) NOT NULL,
	[PI_BKR_ATN_SND_LTR] [char](1) NOT NULL,
	[PI_BKR_LTR_OVR] [char](1) NOT NULL
) ON [PRIMARY]

GO