CREATE TABLE [compfafsa].[StudentData](
	[StudentDataId] [int] IDENTITY(1,1) NOT NULL,
	[StudentFirstName] [varchar](12) NOT NULL,
	[StudentLastName] [varchar](16) NOT NULL,
	[StudentDateOfBirth] [varbinary](300) NOT NULL,
	[SchoolNameFromFile] [varchar](50) NOT NULL,
	[SchoolIdFromFile] [varchar](12) NOT NULL,
	[MasterSchoolListId] [int] NULL,
	[InvalidatedAt] [datetime] NULL,
	[InvalidatedBy] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO