CREATE TABLE [dbo].[ActivityLog](
	[ActivityLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT (getdate()),
	[ProjectFileId] [int] NOT NULL,
	[RunHistoryId] [int] NOT NULL,
	[SourcePath] [nvarchar](512) NOT NULL,
	[DestinationPath] [nvarchar](512) NOT NULL,
	[DecryptionSuccessful] [bit] NULL,
	[CopySuccessful] [bit] NOT NULL,
	[FixLineEndingsSuccessful] [bit] NULL,
	[DeleteSuccessful] [bit] NULL,
	[EncryptionSuccessful] [bit] NULL,
	[CompressionSuccessful] [bit] NULL,
	[Successful] [bit] NOT NULL,
	[InvalidFileId] [int] NULL,
	[PreDecryptionArchiveLocation] [nvarchar](512) NULL,
	[PreEncryptionArchiveLocation] [nvarchar](512) NULL,
 CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED 
(
	[ActivityLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ActivityLog]  WITH NOCHECK ADD  CONSTRAINT [FK_ActivityLog_InvalidFiles] FOREIGN KEY([InvalidFileId])
REFERENCES [dbo].[InvalidFiles] ([InvalidFileId])
GO

ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_InvalidFiles]
GO

ALTER TABLE [dbo].[ActivityLog]  WITH NOCHECK ADD  CONSTRAINT [FK_ActivityLog_ProjectFiles] FOREIGN KEY([ProjectFileId])
REFERENCES [dbo].[ProjectFiles] ([ProjectFileId])
GO

ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_ProjectFiles]
GO

ALTER TABLE [dbo].[ActivityLog]  WITH NOCHECK ADD  CONSTRAINT [FK_ActivityLog_RunHistory] FOREIGN KEY([RunHistoryId])
REFERENCES [dbo].[RunHistory] ([RunHistoryId])
GO

ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_RunHistory]
GO
