CREATE TABLE [dbo].[ProjectFiles](
	[ProjectFileId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[SourceRoot] [nvarchar](256) NOT NULL,
	[SourcePathTypeId] [int] NOT NULL DEFAULT ((0)),
	[DestinationRoot] [nvarchar](256) NOT NULL,
	[DestinationPathTypeId] [int] NOT NULL DEFAULT ((0)),
	[SearchPattern] [nvarchar](128) NULL,
	[AntiSearchPattern] [nvarchar](128) NULL,
	[DecryptFile] [bit] NOT NULL,
	[CompressFile] [bit] NOT NULL,
	[EncryptFile] [bit] NULL,
	[AggregationFormatString] [nvarchar](64) NULL,
	[RenameFormatString] [nvarchar](64) NULL,
	[FixLineEndings] [bit] NOT NULL DEFAULT ((0)),
	[Retired] [bit] NOT NULL DEFAULT ((0)),
	[IsArchiveJob] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_ProjectFiles] PRIMARY KEY CLUSTERED 
(
	[ProjectFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ProjectFiles]  WITH NOCHECK ADD  CONSTRAINT [FK_ProjectFiles_PathTypesDestination] FOREIGN KEY([DestinationPathTypeId])
REFERENCES [dbo].[PathTypes] ([PathTypeId])
GO

ALTER TABLE [dbo].[ProjectFiles] CHECK CONSTRAINT [FK_ProjectFiles_PathTypesDestination]
GO

ALTER TABLE [dbo].[ProjectFiles]  WITH NOCHECK ADD  CONSTRAINT [FK_ProjectFiles_PathTypesSource] FOREIGN KEY([SourcePathTypeId])
REFERENCES [dbo].[PathTypes] ([PathTypeId])
GO

ALTER TABLE [dbo].[ProjectFiles] CHECK CONSTRAINT [FK_ProjectFiles_PathTypesSource]
GO

ALTER TABLE [dbo].[ProjectFiles]  WITH CHECK ADD  CONSTRAINT [FK_ProjectFiles_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
GO

ALTER TABLE [dbo].[ProjectFiles] CHECK CONSTRAINT [FK_ProjectFiles_Projects]
GO

ALTER TABLE [dbo].[ProjectFiles]  WITH CHECK ADD  CONSTRAINT [CK_ProjectFiles_CompressAggregate] CHECK  (([AggregationFormatString] IS NULL OR [CompressFile]=(1)))
GO

ALTER TABLE [dbo].[ProjectFiles] CHECK CONSTRAINT [CK_ProjectFiles_CompressAggregate]
GO

ALTER TABLE [dbo].[ProjectFiles]  WITH CHECK ADD  CONSTRAINT [CK_ProjectFiles_FormatStrings] CHECK  (([AggregationFormatString] IS NULL OR [RenameFormatString] IS NULL))
GO

ALTER TABLE [dbo].[ProjectFiles] CHECK CONSTRAINT [CK_ProjectFiles_FormatStrings]
GO

ALTER TABLE [dbo].[ProjectFiles]  WITH CHECK ADD  CONSTRAINT [CK_ProjectFiles_Patterns] CHECK  (([SearchPattern] IS NOT NULL OR [AntiSearchPattern] IS NOT NULL))
GO

ALTER TABLE [dbo].[ProjectFiles] CHECK CONSTRAINT [CK_ProjectFiles_Patterns]
GO