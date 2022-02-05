CREATE TABLE [print].[PrintProcessingCoBorrower](
	[PrintProcessingId] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [char](10) NOT NULL,
	[EmailAddress] [varchar](256) NULL,
	[ScriptDataId] [int] NOT NULL,
	[SourceFile] [varchar](100) NULL,
	[LetterData] [varchar](max) NOT NULL,
	[CostCenter] [varchar](10) NOT NULL,
	[DoNotProcessEcorr] [bit] NOT NULL DEFAULT ((0)),
	[OnEcorr] [bit] NOT NULL DEFAULT ((0)),
	[ArcAddProcessingId] [bigint] NULL,
	[ArcNeeded] [bit] NOT NULL DEFAULT ((0)),
	[ImagedAt] [datetime] NULL,
	[ImagingNeeded] [bit] NOT NULL DEFAULT ((0)),
	[EcorrDocumentCreatedAt] [datetime] NULL,
	[PrintedAt] [datetime] NULL,
	[AddedBy] [varchar](50) NOT NULL,
	[AddedAt] [datetime] NOT NULL DEFAULT (getdate()),
	[DeletedAt] [date] NULL,
	[DeletedBy] [varchar](50) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[PrintProcessingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [print].[PrintProcessingCoBorrower]  WITH NOCHECK ADD FOREIGN KEY([ScriptDataId])
REFERENCES [print].[ScriptData] ([ScriptDataId])
GO

ALTER TABLE [print].[PrintProcessingCoBorrower]  WITH NOCHECK ADD FOREIGN KEY([ScriptDataId])
REFERENCES [print].[ScriptData] ([ScriptDataId])
GO