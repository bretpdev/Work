CREATE TABLE [acs].[OneLinkDemographics](
	[OneLinkDemographicsId] [int] IDENTITY(1,1) NOT NULL,
	[PersonType] [char](1) NOT NULL,
	[SSN] [varchar](9) NOT NULL,
	[AddrDate] [varchar](4) NULL,
	[AddrType] [char](1) NULL,
	[FirstFourName] [char](4) NULL,
	[FullName] [varchar](32) NULL,
	[Address1] [varchar](255) NULL,
	[Address2] [varchar](255) NULL,
	[City] [varchar](32) NULL,
	[State] [varchar](2) NULL,
	[Zip] [varchar](9) NULL,
	[NewAddressFull] [varchar](150) NULL,
	[OldAddressFull] [varchar](150) NULL,
	[ArcAddProcessingId] [bigint] NULL,
	[FileId] [varchar](16) NOT NULL,
	[ProcessedAt] [datetime] NULL,
	[AddedBy] [varchar](50) NOT NULL,
	[AddedAt] [datetime] NOT NULL,
	[DeletedAt] [date] NULL,
	[DeletedBy] [varchar](50) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[OneLinkDemographicsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [acs].[OneLinkDemographics] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO
