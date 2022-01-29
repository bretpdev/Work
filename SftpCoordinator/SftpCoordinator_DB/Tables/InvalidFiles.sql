CREATE TABLE [dbo].[InvalidFiles](
	[InvalidFileId] [int] IDENTITY(1,1) NOT NULL,
	[FilePath] [nvarchar](512) NOT NULL,
	[FileTimestamp] [datetime] NOT NULL,
	[ErrorMessage] [nvarchar](max) NOT NULL,
	[ResolvedBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[InvalidFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO