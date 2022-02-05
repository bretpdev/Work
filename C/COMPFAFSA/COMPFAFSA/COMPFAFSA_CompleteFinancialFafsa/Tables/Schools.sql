CREATE TABLE [compfafsa].[Schools](
	[MasterSchoolListId] [int] IDENTITY(1,1) NOT NULL,
	[SchoolId] [varchar](12) NOT NULL,
	[DistrictId] [int] NULL,
	[SchoolName] [varchar](200) NOT NULL,
	[StreetAddress] [varchar](200) NULL,
	[City] [varchar](50) NULL,
	[AddedAt] [datetime] NULL,
	[AddedBy] [varchar](200) NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[MasterSchoolListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [compfafsa].[Schools] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO

ALTER TABLE [compfafsa].[Schools] ADD  DEFAULT (suser_sname()) FOR [AddedBy]
GO


