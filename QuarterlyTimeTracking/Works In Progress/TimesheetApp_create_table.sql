drop table CSYS.dbo.COST_DAT_TimesheetApp;

--SELECT 
--	[TimesheetProcessingId] AS TimesheetAppId
--	--,[SourceFile]
--	--,[RowNumber]
--	,[TaskDate]
--	,[Hours]
--	,[Sr]
--	,[Sasr]
--	,[Lts]
--	,[Pmd]
--	,[Project]
--	,[GenericMeetings]
--	,[BatchScripts]
--	,[FsaCr]
--	,[BillingScript]
--	,[ConversionActivities]
--	,[CostCenter]
--	,[Agent]
--	,[CostCenterId]
--	,[SqlUserId]
--	--,SYSTEM_USER AS AddedBy
--	--,GETDATE() AS AddedAt
--INTO 
--	CSYS.dbo.COST_DAT_TimesheetApp
--FROM
--	[CSYS].[dbo].[COST_DAT_TimesheetProcessing]
--where
--	1=0
--;

--ALTER TABLE CSYS.dbo.COST_DAT_TimesheetApp
--ADD AddedBy VARCHAR(100) NULL
--DEFAULT SYSTEM_USER
--;

select * from CSYS.dbo.COST_DAT_TimesheetApp;






USE [CSYS]
GO

/****** SOURCE Object:  SOURCE Table [dbo].[COST_DAT_TimesheetProcessing]    Script Date: 12/28/2018 12:29:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[COST_DAT_TimesheetApp](
	[TimesheetAppId] [int] IDENTITY(1,1) NOT NULL,
	--[SourceFile] [varchar](255) NOT NULL,
	--[RowNumber] [int] NOT NULL,
	[TaskDate] [date] NOT NULL,
	[Hours] [decimal](18, 3) NOT NULL,
	[Sr] [int] NULL,
	[Sasr] [int] NULL,
	[Lts] [int] NULL,
	[Pmd] [int] NULL,
	[Project] [int] NULL,
	[GenericMeetings] [varchar](max) NULL,
	[BatchScripts] [varchar](50) NULL,
	[FsaCr] [varchar](50) NULL,
	[BillingScript] [char](1) NULL,
	[ConversionActivities] [char](1) NULL,
	[CostCenter] [varchar](50) NULL,
	[Agent] [varchar](50) NULL,
	[CostCenterId] [int] NULL,
	[SqlUserId] [int] NULL,
	[AddedBy] [varchar](100) NULL DEFAULT SYSTEM_USER,
 CONSTRAINT [PK_COST_DAT_TimesheetApp] PRIMARY KEY CLUSTERED 
(
	[TimesheetAppId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO


