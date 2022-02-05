
CREATE TABLE [scra].[ActiveDutyReporting](
	[ActiveDutyReportingId] [bigint] IDENTITY(1,1) NOT NULL,
	[BorrSSN] [char](9) NOT NULL,
	[EndrSSN] [char](9) NULL,
	[IsEndorser] bit NOT NULL DEFAULT 0 ,
	[DODBegin] [date] NOT NULL,
	[DODEnd] [date] NOT NULL,
	[TXCXBegin] [date] NOT NULL,
	[TXCXEnd] [date] NOT NULL,
	[TXCXType] [varchar](10) NOT NULL,
	[ServiceComponent] [char](2) NULL,
	[TXCXUpdated] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
 [ErroredAt] DATETIME NULL, 
    CONSTRAINT [PK_ActiveDutyReporting] PRIMARY KEY CLUSTERED 
(
	[ActiveDutyReportingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [scra].[ActiveDutyReporting] ADD  CONSTRAINT [DF_ActiveDutyReporting_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [scra].[ActiveDutyReporting] ADD  CONSTRAINT [DF_ActiveDutyReporting_CreatedBy]  DEFAULT (suser_sname()) FOR [CreatedBy]
GO
