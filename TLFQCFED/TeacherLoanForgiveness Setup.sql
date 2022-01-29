USE [CLS]
GO

/****** Object:  Table [dbo].[ClosedSchoolApp]    Script Date: 10/18/2019 11:48:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE SCHEMA [tlfqcfed]
GO

CREATE TABLE [tlfqcfed].[TeacherLoanForgiveness](
	[TeacherLoanForgivenessId] [int] IDENTITY(1,1) NOT NULL,
	[DF_SPE_ACC_ID] [char](10) NOT NULL,
	[WF_QUE] [char](2) NOT NULL,
	[WF_SUB_QUE] [char](2) NOT NULL,
	[WN_CTL_TSK] [char](18) NOT NULL,
	[PF_REQ_ACT] [char](5) NOT NULL,
	[WD_ACT_REQ] [datetime] NOT NULL,
	[WC_STA_WQUE20] [char](1) NOT NULL,
	[WF_LST_DTS_WQ20] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TeacherLoanForgivenessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


