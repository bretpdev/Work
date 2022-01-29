USE [CLS]
GO

/****** Object:  Table [dbo].[LetterToEmailProcessing]    Script Date: 5/24/2019 3:02:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LetterToEmailProcessing](
	[LetterToEmailProcessingId] [bigint] IDENTITY(1,1) NOT NULL,
	[RM_APL_PGM_PRC] [char](8) NOT NULL,
	[RT_RUN_SRT_DTS_PRC] [datetime2](7) NOT NULL,
	[RN_SEQ_LTR_CRT_PRC] [int] NOT NULL,
	[RN_SEQ_REC_PRC] [smallint] NOT NULL,
	[RM_DSC_LTR_PRC] [char](10) NOT NULL,
	[EmailCampaignId] [int] NOT NULL,
	[Recipient] [varchar](254) NOT NULL,
	[AccountNumber] [char](10) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[LineData] [varchar](MAX) NULL,
	[AddedAt] [datetime] NOT NULL,
	[AddedBy] [varchar](100) NOT NULL,
	[IsCoborrowerRecord] [bit] NOT NULL,
	[ProcessedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](100) NULL,
 CONSTRAINT [PK_LetterToEmailProcessing] PRIMARY KEY CLUSTERED 
(
	[LetterToEmailProcessingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO


