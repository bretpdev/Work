CREATE TABLE [compfafsa].[SchoolVariations](
	[SchoolVariationsId] [int] IDENTITY(1,1) NOT NULL,
	[SchoolIdFromFile] [varchar](12) NOT NULL,
	[SchoolName] [varchar](200) NOT NULL,
	[AdjustedSchoolId] [varchar](12) NULL,
	[AddedAt] [datetime] NULL,
	[AddedBy] [varchar](200) NULL,
	[AdjustedAt] [datetime] NULL,
	[AdjustedBy] [varchar](200) NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[SchoolVariationsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [compfafsa].[SchoolVariations] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO

ALTER TABLE [compfafsa].[SchoolVariations] ADD  DEFAULT (suser_sname()) FOR [AddedBy]
GO


