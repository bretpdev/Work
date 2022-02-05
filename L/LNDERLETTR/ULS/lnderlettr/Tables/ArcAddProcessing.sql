CREATE TABLE [dbo].[ArcAddProcessing](
	[ArcAddProcessingId] [bigint] IDENTITY(1,1) NOT NULL,
	[ArcTypeId] [int] NOT NULL,
	[ArcResponseCodeId] [int] NULL,
	[AccountNumber] [char](10) NOT NULL,
	[RecipientId] [char](9) NULL,
	[ARC] [varchar](5) NOT NULL,
	[ActivityType] [varchar](2) NULL,
	[ActivityContact] [varchar](2) NULL,
	[ScriptId] [char](10) NOT NULL,
	[ProcessOn] [datetime] NOT NULL,
	[Comment] [varchar](1233) NULL,
	[IsReference] [bit] NOT NULL,
	[IsEndorser] [bit] NOT NULL,
	[ProcessFrom] [datetime] NULL,
	[ProcessTo] [datetime] NULL,
	[NeededBy] [datetime] NULL,
	[RegardsTo] [char](9) NULL,
	[RegardsCode] [char](1) NULL,
	[LN_ATY_SEQ] [int] NULL,
	[ProcessingAttempts] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[ProcessedAt] [datetime] NULL,
 CONSTRAINT [PK_ArcAddProcessing] PRIMARY KEY CLUSTERED 
(
	[ArcAddProcessingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ArcAddProcessing] ADD  DEFAULT ((0)) FOR [ProcessingAttempts]
GO

ALTER TABLE [dbo].[ArcAddProcessing] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[ArcAddProcessing] ADD  DEFAULT (suser_sname()) FOR [CreatedBy]
GO
