﻿CREATE TABLE [ECORRINERR].[ErrorRecords](
	[ErrorRecordId] [bigint] IDENTITY(1,1) NOT NULL,
	[Path] [varchar](max) NULL,
	[SSN] [char](9) NULL,
	[DOC_DATE] [datetime] NULL,
	[DOC_ID] [varchar](10) NULL,
	[ADDR_ACC_NUM] [varchar](10) NULL,
	[LETTER_ID] [varchar](10) NULL,
	[REQUEST_USER] [varchar](8) NULL,
	[VIEWABLE] [char](1) NULL,
	[CORR_METHOD] [varchar](20) NULL,
	[REPORT_DESC] [varchar](60) NULL,
	[LOAD_TIME] [datetime] NULL,
	[REPORT_NAME] [varchar](17) NULL,
	[ADDRESSEE_EMAIL] [varchar](254) NULL,
	[VIEWED] [char](1) NULL,
	[CREATE_DATE] [datetime] NULL,
	[MAINFRAME_REGION] [varchar](8) NULL,
	[DCN] [varchar](17) NULL,
	[SUBJECT_LINE] [varchar](50) NULL,
	[DOC_SOURCE] [varchar](10) NULL,
	[DOC_COMMENT] [varchar](255) NULL,
	[WORKFLOW] [char](1) NULL,
	[DOC_DELETE] [char](1) NULL,
	[Region] [varchar](12) NULL,
	[ErrorText] [varchar](max) NULL,
	[ErrorFileName] [varchar](max) NULL, 
    [ResolvedAt] DATETIME NULL, 
    [ResolvedBy] VARCHAR(50) NULL, 
    CONSTRAINT [PK_ErrorRecords] PRIMARY KEY ([ErrorRecordId])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]